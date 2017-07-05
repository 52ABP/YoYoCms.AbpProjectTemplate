using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Zero.Configuration;
using YoYoCms.AbpProjectTemplate.Configuration;
using YoYoCms.AbpProjectTemplate.Debugging;
using YoYoCms.AbpProjectTemplate.Web.Models.TenantRegistration;
using Abp.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.Editions;
using YoYoCms.AbpProjectTemplate.MultiTenancy;
using YoYoCms.AbpProjectTemplate.Notifications;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;
using YoYoCms.AbpProjectTemplate.Web.Authorization;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
    public class TenantRegistrationController : AbpProjectTemplateControllerBase
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly LogInManager _logInManager;
        private readonly EditionManager _editionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public TenantRegistrationController(
            IMultiTenancyConfig multiTenancyConfig,
            TenantManager tenantManager,
            EditionManager editionManager,
            IAppNotifier appNotifier,
            UserManager userManager,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            LogInManager logInManager)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _appNotifier = appNotifier;
            _userManager = userManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
        }

        public ActionResult Index()
        {
            CheckTenantRegistrationIsEnabled();

            ViewBag.UseCaptcha = UseCaptchaOnRegistration();
            ViewBag.PasswordComplexitySetting = SettingManager.GetSettingValue(AppSettings.Security.PasswordComplexity).Replace("\"","");

            return View();
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> Register(TenantRegistrationViewModel model)
        {
            try
            {
                CheckTenantRegistrationIsEnabled();

                if (UseCaptchaOnRegistration())
                {
                    if (model.Captcha.IsNullOrWhiteSpace())
                    {
                        throw new UserFriendlyException(L("CaptchaCanNotBeEmpty"));
                    }

                    var result = VerifyTheCaptcha(model.Captcha);
                    if (result)
                    {
                        return Json(true);
                    }
                    throw new UserFriendlyException(L("IncorrectCaptchaAnswer"));
                    //todo:租户注册的时候需要验证码
                    //var recaptchaHelper = this.GetRecaptchaVerificationHelper();
                    //if (recaptchaHelper.Response.IsNullOrEmpty())
                    //{
                    //    throw new UserFriendlyException(L("CaptchaCanNotBeEmpty"));
                    //}

                    //if (recaptchaHelper.VerifyRecaptchaResponse() != RecaptchaVerificationResult.Success)
                    //{
                    //    throw new UserFriendlyException(L("IncorrectCaptchaAnswer"));
                    //}
                }

                //Getting host-specific settings
                var isNewRegisteredTenantActiveByDefault = await SettingManager.GetSettingValueForApplicationAsync<bool>(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault);
                var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
                var defaultEditionIdValue = await SettingManager.GetSettingValueForApplicationAsync(AppSettings.TenantManagement.DefaultEdition);
                int? defaultEditionId = null;

                if (!string.IsNullOrEmpty(defaultEditionIdValue) && (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionIdValue)) != null))
                {
                    defaultEditionId = Convert.ToInt32(defaultEditionIdValue);
                }

                CurrentUnitOfWork.SetTenantId(null);

                var tenantId = await _tenantManager.CreateWithAdminUserAsync(
                  model.TenancyName,
                  model.Name,
                  model.AdminPassword,
                  model.AdminEmailAddress,
                  null,
                  isNewRegisteredTenantActiveByDefault,
                  defaultEditionId,
                  false,
                  true);

                ViewBag.UseCaptcha = UseCaptchaOnRegistration();

                var tenant = await _tenantManager.GetByIdAsync(tenantId);
                await _appNotifier.NewTenantRegisteredAsync(tenant);

                CurrentUnitOfWork.SetTenantId(tenant.Id);

                var user = await _userManager.FindByNameAsync(UserManagement.Users.User.AdminUserName);

                //Directly login if possible
                if (tenant.IsActive && user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin))
                {
                    var loginResult = await GetLoginResultAsync(user.UserName, model.AdminPassword, tenant.TenancyName);

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
                        return Redirect(Url.Action("Index", "Application"));
                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                return View("RegisterResult", new TenantRegisterResultViewModel
                {
                    TenancyName = model.TenancyName,
                    Name = model.Name,
                    UserName = UserManagement.Users.User.AdminUserName,
                    EmailAddress = model.AdminEmailAddress,
                    IsActive = isNewRegisteredTenantActiveByDefault,
                    IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.UseCaptcha = UseCaptchaOnRegistration();
                ViewBag.ErrorMessage = ex.Message;

                return View("Index", model);
            }
        }

        private bool IsSelfRegistrationEnabled()
        {
            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.AllowSelfRegistration);
        }

        private void CheckTenantRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfTenantRegistrationIsDisabledMessage_Detail"));
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                throw new UserFriendlyException(L("MultiTenancyIsNotEnabled"));
            }
        }

        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration);
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            AuthenticationManager.SignOutAllAndSignIn(identity, rememberMe);
        }
    }
}