using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Notifications;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Models;
using Abp.Web.Security.AntiForgery;
using Abp.Zero.Configuration;
using LTM.Common.Drawing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.Authorization.Impersonation;
using YoYoCms.AbpProjectTemplate.Authorization.Roles;
using YoYoCms.AbpProjectTemplate.Configuration;
using YoYoCms.AbpProjectTemplate.Debugging;
using YoYoCms.AbpProjectTemplate.MultiTenancy;
using YoYoCms.AbpProjectTemplate.Notifications;
using YoYoCms.AbpProjectTemplate.Web.Controllers.Results;
using YoYoCms.AbpProjectTemplate.Web.Models.Account;
using YoYoCms.AbpProjectTemplate.Web.MultiTenancy;

using Newtonsoft.Json;
using YoYoCms.AbpProjectTemplate.Security;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;
using YoYoCms.AbpProjectTemplate.Web.Authorization;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
    public class AccountController : AbpProjectTemplateControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly TenantManager _tenantManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IUserEmailer _userEmailer;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITenancyNameFinder _tenancyNameFinder;
        private readonly ICacheManager _cacheManager;
        private readonly IWebUrlService _webUrlService;
        private readonly IAppNotifier _appNotifier;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IUserLinkManager _userLinkManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly SignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILanguageManager _languageManager;

        public AccountController(
            LogInManager logInManager,
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IUserEmailer userEmailer,
            RoleManager roleManager,
            TenantManager tenantManager,
            IUnitOfWorkManager unitOfWorkManager,
            ITenancyNameFinder tenancyNameFinder,
            ICacheManager cacheManager,
            IAppNotifier appNotifier,
            IWebUrlService webUrlService,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            IUserLinkManager userLinkManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            SignInManager signInManager,
            IAuthenticationManager authenticationManager,
            ILanguageManager languageManager)
        {
            _userManager = userManager;
            _multiTenancyConfig = multiTenancyConfig;
            _userEmailer = userEmailer;
            _roleManager = roleManager;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _tenancyNameFinder = tenancyNameFinder;
            _cacheManager = cacheManager;
            _webUrlService = webUrlService;
            _appNotifier = appNotifier;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userLinkManager = userLinkManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _languageManager = languageManager;
            _logInManager = logInManager;
        }

        #region Login / Logout

        public ActionResult Login(string userNameOrEmailAddress = "", string returnUrl = "", string successMessage = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View(
                new LoginFormViewModel
                {
                    TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull(),
                    IsSelfRegistrationEnabled = IsSelfRegistrationEnabled(),
                    SuccessMessage = successMessage,
                    UserNameOrEmailAddress = userNameOrEmailAddress
                });
        }

        /// <summary>
        /// 登陆逻辑验证
        /// </summary>
        /// <param name="loginModel"></param>
        /// <param name="returnUrl"></param>
        /// <param name="returnUrlHash"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password, loginModel.TenancyName);

            var tenantId = loginResult.Tenant == null ? (int?)null : loginResult.Tenant.Id;

            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                if (loginResult.User.ShouldChangePasswordOnNextLogin)
                {
                    loginResult.User.SetNewPasswordResetCode();

                    return Json(new AjaxResponse
                    {
                        TargetUrl = Url.Action(
                            "ResetPassword",
                            new ResetPasswordViewModel
                            {
                                TenantId = SimpleStringCipher.Instance.Encrypt(tenantId == null ? null : tenantId.ToString()),
                                UserId = SimpleStringCipher.Instance.Encrypt(loginResult.User.Id.ToString()),
                                ResetCode = loginResult.User.PasswordResetCode
                            }),
                        Result = new
                        {
                            TenantId = SimpleStringCipher.Instance.Encrypt(tenantId == null ? null : tenantId.ToString()),
                            UserId = SimpleStringCipher.Instance.Encrypt(loginResult.User.Id.ToString()),
                            ResetCode = loginResult.User.PasswordResetCode,
                            ResetPassword = true
                        }
                    });
                }

                var listClaims = AddIdentityInfo(loginResult.Identity, loginResult.User);

                loginResult.Identity.AddClaims(listClaims);

                var signInResult = await _signInManager.SignInOrTwoFactor(loginResult, loginModel.RememberMe);
                if (signInResult == SignInStatus.RequiresVerification)
                {
                    return Json(new AjaxResponse
                    {
                        TargetUrl = Url.Action(
                            "SendSecurityCode",
                            new
                            {
                                returnUrl = returnUrl,
                                rememberMe = loginModel.RememberMe
                            })
                    });
                }

                Debug.Assert(signInResult == SignInStatus.Success);

                await UnitOfWorkManager.Current.SaveChangesAsync();

                return Json(new AjaxResponse { TargetUrl = returnUrl });
            }
        }

        public ActionResult Logout()
        {
            _authenticationManager.SignOutAll();
            return RedirectToAction("Index","Home");
        }
        /// <summary>
        /// 注册到系统中
        /// </summary>
        /// <param name="user"></param>
        /// <param name="identity"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }



            _authenticationManager.SignOutAllAndSignIn(identity, rememberMe);
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

        #endregion

        #region Two Factor Auth

        public async Task<ActionResult> SendSecurityCode(string returnUrl, bool rememberMe)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId <= 0)
            {
                return RedirectToAction("Login");
            }

            var tenantId = await _signInManager.GetVerifiedTenantIdAsync();
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var userProviders = await _userManager.GetValidTwoFactorProvidersAsync(userId);

                var factorOptions = userProviders.Select(
                    userProvider =>
                        new SelectListItem
                        {
                            Text = userProvider,
                            Value = userProvider
                        }).ToList();

                return View(
                    new SendSecurityCodeViewModel
                    {
                        Providers = factorOptions,
                        ReturnUrl = returnUrl,
                        RememberMe = rememberMe
                    }
                );
            }
        }

        [HttpPost]
        public async Task<ActionResult> SendSecurityCode(SendSecurityCodeViewModel model)
        {
            var tenantId = await _signInManager.GetVerifiedTenantIdAsync();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                {
                    throw new UserFriendlyException(L("SendSecurityCodeErrorMessage"));
                }

                return RedirectToAction(
                    "VerifySecurityCode",
                    new
                    {
                        provider = model.SelectedProvider,
                        returnUrl = model.ReturnUrl,
                        rememberMe = model.RememberMe
                    }
                );
            }
        }

        public async Task<ActionResult> VerifySecurityCode(string provider, string returnUrl, bool rememberMe)
        {
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                throw new UserFriendlyException(L("VerifySecurityCodeNotLoggedInErrorMessage"));
            }

            var tenantId = await _signInManager.GetVerifiedTenantIdAsync();

            var isRememberBrowserEnabled = tenantId == null
                ? await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled)
                : await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, tenantId.Value);

            return View(
                new VerifySecurityCodeViewModel
                {
                    Provider = provider,
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe,
                    IsRememberBrowserEnabled = isRememberBrowserEnabled
                }
            );
        }

        [HttpPost]
        public async Task<JsonResult> VerifySecurityCode(VerifySecurityCodeViewModel model)
        {
            model.ReturnUrl = NormalizeReturnUrl(model.ReturnUrl);

            var tenantId = await _signInManager.GetVerifiedTenantIdAsync();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var result = await _signInManager.TwoFactorSignInAsync(
                    model.Provider,
                    model.Code,
                    isPersistent: model.RememberMe,
                    rememberBrowser: model.RememberBrowser
                );

                switch (result)
                {
                    case SignInStatus.Success:
                        return Json(new AjaxResponse { TargetUrl = model.ReturnUrl });
                    case SignInStatus.LockedOut:
                        throw new UserFriendlyException(L("UserLockedOutMessage"));
                    case SignInStatus.Failure:
                    default:
                        throw new UserFriendlyException(L("InvalidSecurityCode"));
                }
            }
        }

        #endregion


        #region LoginResult.Identity扩展加载

        /// <summary>
        /// Identity身份信息拓展
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<Claim> AddIdentityInfo(ClaimsIdentity identity, User user)
        {

            var list = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(AbpProjectTemplateConsts.ClaimTypes.UserName, user.UserName)
             
            };

            return list;

        }






        #endregion

        #region Register

        public ActionResult Register()
        {
            return RegisterView(new RegisterViewModel
            {
                TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull(),
                PasswordComplexitySetting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(SettingManager.GetSettingValue(AppSettings.Security.PasswordComplexity))
            });
        }
        /// <summary>
        /// 注册后的页面
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RegisterView(RegisterViewModel model)
        {
            CheckSelfRegistrationIsEnabled();

            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();

            return View("Register", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type">default-默认提交 ajax</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> Register(RegisterViewModel model, string type = "default")
        {
            try
            {
                CheckSelfRegistrationIsEnabled();

                if (!model.IsExternalLogin && UseCaptchaOnRegistration())
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

                    //todo:制作验证码功能
                    //  var recaptchaHelper = this.GetRecaptchaVerificationHelper();

                    //if (recaptchaHelper.Response.IsNullOrEmpty())
                    //{
                    //    throw new UserFriendlyException(L("CaptchaCanNotBeEmpty"));
                    //}

                    //if (recaptchaHelper.VerifyRecaptchaResponse() != RecaptchaVerificationResult.Success)
                    //{
                    //    throw new UserFriendlyException(L("IncorrectCaptchaAnswer"));
                    //}
                }

                if (!_multiTenancyConfig.IsEnabled)
                {
                    model.TenancyName = Tenant.DefaultTenantName;
                }
                else if (model.TenancyName.IsNullOrEmpty())
                {
                    throw new UserFriendlyException(L("TenantNameCanNotBeEmpty"));
                }

                CurrentUnitOfWork.SetTenantId(null);

                var tenant = await GetActiveTenantAsync(model.TenancyName);

                CurrentUnitOfWork.SetTenantId(tenant.Id);

                if (!await SettingManager.GetSettingValueForTenantAsync<bool>(AppSettings.UserManagement.AllowSelfRegistration, tenant.Id))
                {
                    throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
                }

                //Getting tenant-specific settings
                var isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueForTenantAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault, tenant.Id);
                var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenant.Id);

                var user = new User
                {
                    TenantId = tenant.Id,
                    Name = model.Name,
                    Surname = model.Surname,
                    EmailAddress = model.EmailAddress,
                    IsActive = isNewRegisteredUserActiveByDefault
                };

                ExternalLoginInfo externalLoginInfo = null;
                if (model.IsExternalLogin)
                {
                    externalLoginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        throw new ApplicationException("Can not external login!");
                    }

                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            LoginProvider = externalLoginInfo.Login.LoginProvider,
                            ProviderKey = externalLoginInfo.Login.ProviderKey,
                            TenantId = tenant.Id
                        }
                    };

                    model.UserName = model.EmailAddress;
                    model.Password = UserManagement.Users.User.CreateRandomPassword();

                    if (string.Equals(externalLoginInfo.Email, model.EmailAddress, StringComparison.InvariantCultureIgnoreCase))
                    {
                        user.IsEmailConfirmed = true;
                    }
                }
                else
                {
                    if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(L("FormIsNotValidMessage"));
                    }
                }

                user.UserName = model.UserName;
                user.Password = new PasswordHasher().HashPassword(model.Password);

                user.Roles = new List<UserRole>();
                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
                }

                CheckErrors(await _userManager.CreateAsync(user));
                try
                {
                    await _unitOfWorkManager.Current.SaveChangesAsync();

             
                    //email可空
                if (!user.IsEmailConfirmed)
                {
                //    user.SetNewEmailConfirmationCode();
                  //  await _userEmailer.SendEmailActivationLinkAsync(user);
                }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                //Notifications
                await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);
                await _appNotifier.NewUserRegisteredAsync(user);

                //Directly login if possible
                if (user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin))
                {
                    AbpLoginResult<Tenant, User> loginResult;
                    if (externalLoginInfo != null)
                    {
                        loginResult = await _logInManager.LoginAsync(externalLoginInfo.Login, tenant.TenancyName);
                    }
                    else
                    {
                        loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenant.TenancyName);
                    }

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
	                    return Json(new AjaxResponse(new RegisterResultViewModel()
	                    {
		                    IsActive = user.IsActive,
		                    IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin
	                    }));

                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                // 如果是ajax请求
                if (type == "ajax")
                    return Json(new AjaxResponse(new RegisterResultViewModel()
                    {
                        IsActive = user.IsActive,
                        IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin
                    }));

                return View("RegisterResult", new RegisterResultViewModel
                {
                    TenancyName = tenant.TenancyName,
                    NameAndSurname = user.Name + " " + user.Surname,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive,
                    IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin
                });
            }
            catch (UserFriendlyException ex)
            {
                // 如果是ajax请求
                if (type == "ajax")
                    return Json(new AjaxResponse() { Success = false, Error = new ErrorInfo(ex.Message) });

                ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
                ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();
                ViewBag.ErrorMessage = ex.Message;

                return View("Register", model);
            }
        }

        /// <summary>
        /// 是否启用用户注册验证码信息(前端调用判断是否启用验证码)
        /// </summary>
        /// <returns></returns>
        public bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            var tenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();
            if (tenancyName.IsNullOrEmpty())
            {
                return true;
            }

            var tenant = AsyncHelper.RunSync(() => GetActiveTenantAsync(tenancyName));
            return SettingManager.GetSettingValueForTenant<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration, tenant.Id);
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool IsSelfRegistrationEnabled()
        {
            var tenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();
            if (tenancyName.IsNullOrEmpty())
            {
                return true;
            }

            var tenant = AsyncHelper.RunSync(() => GetActiveTenantAsync(tenancyName));
            return SettingManager.GetSettingValueForTenant<bool>(AppSettings.UserManagement.AllowSelfRegistration, tenant.Id);
        }

        #endregion

        #region ForgotPassword / ResetPassword

        public ActionResult ForgotPassword()
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            ViewBag.TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();

            return View();
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> SendPasswordResetLink(SendPasswordResetLinkViewModel model)
        {
            UnitOfWorkManager.Current.SetTenantId(await GetTenantIdOrDefault(model.TenancyName));

            var user = await GetUserByChecking(model.EmailAddress);

            user.SetNewPasswordResetCode();
            await _userEmailer.SendPasswordResetLinkAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse());
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var tenantId = model.TenantId.IsNullOrEmpty() ? (int?)null : SimpleStringCipher.Instance.Decrypt(model.TenantId).To<int>();
            var userId = SimpleStringCipher.Instance.Decrypt(model.UserId).To<long>();

            _unitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            var setting = await SettingManager.GetSettingValueForUserAsync(AppSettings.Security.PasswordComplexity, tenantId, userId);
            model.PasswordComplexitySetting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(setting);

            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordFormViewModel model)
        {
            var tenantId = model.TenantId.IsNullOrEmpty() ? (int?)null : SimpleStringCipher.Instance.Decrypt(model.TenantId).To<int>();
            var userId = Convert.ToInt64(SimpleStringCipher.Instance.Decrypt(model.UserId));

            _unitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            user.Password = new PasswordHasher().HashPassword(model.Password);
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.ShouldChangePasswordOnNextLogin = false;

            await _userManager.UpdateAsync(user);

            if (user.IsActive)
            {
                await SignInAsync(user);
            }

            return RedirectToAction("Index", "Application");
        }

        #endregion

        #region Email activation / confirmation

        public ActionResult EmailActivation()
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            ViewBag.TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();

            return View();
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> SendEmailActivationLink(SendEmailActivationLinkViewModel model)
        {
            var tenantId = await GetTenantIdOrDefault(model.TenancyName);

            UnitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await GetUserByChecking(model.EmailAddress);

            user.SetNewEmailConfirmationCode();
            await _userEmailer.SendEmailActivationLinkAsync(user);

            return Json(new AjaxResponse());
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> EmailConfirmation(EmailConfirmationViewModel model)
        {
            var tenantId = model.TenantId.IsNullOrEmpty() ? (int?)null : SimpleStringCipher.Instance.Decrypt(model.TenantId).To<int>();
            var userId = Convert.ToInt64(SimpleStringCipher.Instance.Decrypt(model.UserId));

            _unitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() || user.EmailConfirmationCode != model.ConfirmationCode)
            {
                throw new UserFriendlyException(L("InvalidEmailConfirmationCode"), L("InvalidEmailConfirmationCode_Detail"));
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;

            await _userManager.UpdateAsync(user);

            var tenancyName = user.TenantId.HasValue
                ? (await _tenantManager.GetByIdAsync(user.TenantId.Value)).TenancyName
                : "";

            return RedirectToAction(
                "Login",
                new
                {
                    successMessage = L("YourEmailIsConfirmedMessage"),
                    tenancyName = tenancyName,
                    userNameOrEmailAddress = user.UserName
                });
        }

        #endregion

        #region Child actions

        [ChildActionOnly]
        public PartialViewResult Languages()
        {
            return PartialView("~/Views/Account/_Languages.cshtml",
                new LanguagesViewModel
                {
                    AllLanguages = _languageManager.GetLanguages(),
                    CurrentLanguage = _languageManager.CurrentLanguage
                });
        }

        #endregion

        #region Private methods

        private async Task<User> GetUserByChecking(string emailAddress)
        {
            var user = await _userManager.Users.Where(
                u => u.EmailAddress == emailAddress
                ).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }

        private async Task<int?> GetTenantIdOrDefault(string tenancyName)
        {
            return tenancyName.IsNullOrEmpty()
                ? AbpSession.TenantId
                : (await GetActiveTenantAsync(tenancyName)).Id;
        }

        private async Task<Tenant> GetActiveTenantAsync(string tenancyName)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIsNotActive", tenancyName));
            }

            return tenant;
        }

        #endregion

        #region External Login

        [HttpPost]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(
                provider,
                Url.Action(
                    "ExternalLoginCallback",
                    "Account",
                    new
                    {
                        ReturnUrl = returnUrl,
                        tenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull() ?? ""
                    })
                );
        }

        [UnitOfWork]
        [DisableAbpAntiForgeryTokenValidation]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string tenancyName = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            //Try to find tenancy name
            if (tenancyName.IsNullOrEmpty())
            {
                tenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();
                if (tenancyName.IsNullOrEmpty())
                {
                    var tenants = await FindPossibleTenantsOfUserAsync(loginInfo.Login);
                    switch (tenants.Count)
                    {
                        case 0:
                            return await RegisterView(loginInfo);
                        case 1:
                            tenancyName = tenants[0].TenancyName;
                            break;
                        default:
                            return View("TenantSelection", new TenantSelectionViewModel
                            {
                                Action = Url.Action("ExternalLoginCallback", "Account", new { returnUrl }),
                                Tenants = tenants.MapTo<List<TenantSelectionViewModel.TenantInfo>>()
                            });
                    }
                }
            }

            var loginResult = await _logInManager.LoginAsync(loginInfo.Login, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    await SignInAsync(loginResult.User, loginResult.Identity, true);
                    return Redirect(returnUrl);
                case AbpLoginResultType.UnknownExternalLogin:
                    return await RegisterView(loginInfo, tenancyName);
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, loginInfo.Email ?? loginInfo.DefaultUserName, tenancyName);
            }
        }

        private async Task<ActionResult> RegisterView(ExternalLoginInfo loginInfo, string tenancyName = null)
        {
            var name = loginInfo.DefaultUserName;
            var surname = loginInfo.DefaultUserName;

            var extractedNameAndSurname = TryExtractNameAndSurnameFromClaims(loginInfo.ExternalIdentity.Claims.ToList(), ref name, ref surname);

            var viewModel = new RegisterViewModel
            {
                TenancyName = tenancyName,
                EmailAddress = loginInfo.Email,
                Name = name,
                Surname = surname,
                IsExternalLogin = true
            };

            if (!tenancyName.IsNullOrEmpty() && extractedNameAndSurname)
            {
                return await Register(viewModel);
            }

            return RegisterView(viewModel);
        }

        [UnitOfWork]
        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;

            //TODO: Store all login information on host db or disable this feature for db-per-tenant architecture

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .Where(t => t != null)
                .ToList();
        }

        private static bool TryExtractNameAndSurnameFromClaims(List<Claim> claims, ref string name, ref string surname)
        {
            string foundName = null;
            string foundSurname = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                foundName = givennameClaim.Value;
            }

            var surnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            if (surnameClaim != null && !surnameClaim.Value.IsNullOrEmpty())
            {
                foundSurname = surnameClaim.Value;
            }

            if (foundName == null || foundSurname == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    var nameSurName = nameClaim.Value;
                    if (!nameSurName.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = nameSurName.LastIndexOf(' ');
                        if (lastSpaceIndex < 1 || lastSpaceIndex > (nameSurName.Length - 2))
                        {
                            foundName = foundSurname = nameSurName;
                        }
                        else
                        {
                            foundName = nameSurName.Substring(0, lastSpaceIndex);
                            foundSurname = nameSurName.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            if (!foundName.IsNullOrEmpty())
            {
                name = foundName;
            }

            if (!foundSurname.IsNullOrEmpty())
            {
                surname = foundSurname;
            }

            return foundName != null && foundSurname != null;
        }

        #endregion

        #region Impersonation

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Impersonation)]
        [DisableAbpAntiForgeryTokenValidation]
        public virtual async Task<JsonResult> Impersonate(ImpersonateModel model)
        {
            if (AbpSession.ImpersonatorUserId.HasValue)
            {
                throw new UserFriendlyException(L("CascadeImpersonationErrorMessage"));
            }

            if (AbpSession.TenantId.HasValue)
            {
                if (!model.TenantId.HasValue)
                {
                    throw new UserFriendlyException(L("FromTenantToHostImpersonationErrorMessage"));
                }

                if (model.TenantId.Value != AbpSession.TenantId.Value)
                {
                    throw new UserFriendlyException(L("DifferentTenantImpersonationErrorMessage"));
                }
            }

            var result = await SaveImpersonationTokenAndGetTargetUrl(model.TenantId, model.UserId, false);
            _authenticationManager.SignOutAll();
            return result;
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> ImpersonateSignIn(string tokenId)
        {
            var cacheItem = await _cacheManager.GetImpersonationCache().GetOrDefaultAsync(tokenId);
            if (cacheItem == null)
            {
                throw new UserFriendlyException(L("ImpersonationTokenErrorMessage"));
            }

            //Switch to requested tenant
            _unitOfWorkManager.Current.SetTenantId(cacheItem.TargetTenantId);

            //Get the user from tenant
            var user = await _userManager.FindByIdAsync(cacheItem.TargetUserId);

            //Create identity
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            if (!cacheItem.IsBackToImpersonator)
            {
                //Add claims for audit logging
                if (cacheItem.ImpersonatorTenantId.HasValue)
                {
                    identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorTenantId, cacheItem.ImpersonatorTenantId.Value.ToString(CultureInfo.InvariantCulture)));
                }

                identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorUserId, cacheItem.ImpersonatorUserId.ToString(CultureInfo.InvariantCulture)));
            }

            _authenticationManager.SignOutAllAndSignIn(identity);

            //Remove the cache item to prevent re-use
            await _cacheManager.GetImpersonationCache().RemoveAsync(tokenId);

            return RedirectToAction("Index", "Application");
        }

        public virtual JsonResult IsImpersonatedLogin()
        {
            return Json(new AjaxResponse { Result = AbpSession.ImpersonatorUserId.HasValue });
        }

        public virtual async Task<JsonResult> BackToImpersonator()
        {
            if (!AbpSession.ImpersonatorUserId.HasValue)
            {
                throw new UserFriendlyException(L("NotImpersonatedLoginErrorMessage"));
            }

            var result = await SaveImpersonationTokenAndGetTargetUrl(AbpSession.ImpersonatorTenantId, AbpSession.ImpersonatorUserId.Value, true);
            _authenticationManager.SignOutAll();
            return result;
        }

        private async Task<JsonResult> SaveImpersonationTokenAndGetTargetUrl(int? tenantId, long userId, bool isBackToImpersonator)
        {
            //Create a cache item
            var cacheItem = new ImpersonationCacheItem(
                tenantId,
                userId,
                isBackToImpersonator
                );

            if (!isBackToImpersonator)
            {
                cacheItem.ImpersonatorTenantId = AbpSession.TenantId;
                cacheItem.ImpersonatorUserId = AbpSession.GetUserId();
            }

            //Create a random token and save to the cache
            var tokenId = Guid.NewGuid().ToString();
            await _cacheManager
                .GetImpersonationCache()
                .SetAsync(tokenId, cacheItem, TimeSpan.FromMinutes(1));

            //Find tenancy name
            string tenancyName = null;
            if (tenantId.HasValue)
            {
                tenancyName = (await _tenantManager.GetByIdAsync(tenantId.Value)).TenancyName;
            }

            //Create target URL
            var targetUrl = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/ImpersonateSignIn?tokenId=" + tokenId;
            return Json(new AjaxResponse { TargetUrl = targetUrl });
        }

        #endregion

        #region Linked Account

        [UnitOfWork]
        [AbpMvcAuthorize]
        public virtual async Task<JsonResult> SwitchToLinkedAccount(SwitchToLinkedAccountModel model)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), model.ToUserIdentifier()))
            {
                throw new ApplicationException(L("This account is not linked to your account"));
            }

            var result = await SaveAccountSwitchTokenAndGetTargetUrl(model.TargetTenantId, model.TargetUserId);
            _authenticationManager.SignOutAll();
            return result;
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> SwitchToLinkedAccountSignIn(string tokenId)
        {
            var cacheItem = await _cacheManager.GetSwitchToLinkedAccountCache().GetOrDefaultAsync(tokenId);
            if (cacheItem == null)
            {
                throw new UserFriendlyException(L("SwitchToLinkedAccountTokenErrorMessage"));
            }

            //Switch to requested tenant
            _unitOfWorkManager.Current.SetTenantId(cacheItem.TargetTenantId);

            //Get the user from tenant
            var user = await _userManager.FindByIdAsync(cacheItem.TargetUserId);

            //Create identity
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            //Add claims for audit logging
            if (cacheItem.ImpersonatorTenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorTenantId, cacheItem.ImpersonatorTenantId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            if (cacheItem.ImpersonatorUserId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorUserId, cacheItem.ImpersonatorUserId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            _authenticationManager.SignOutAllAndSignIn(identity);

            user.LastLoginTime = Clock.Now;

            //Remove the cache item to prevent re-use
            await _cacheManager.GetSwitchToLinkedAccountCache().RemoveAsync(tokenId);

            return RedirectToAction("Index", "Application");
        }

        private async Task<JsonResult> SaveAccountSwitchTokenAndGetTargetUrl(int? targetTenantId, long targetUserId)
        {
            //Create a cache item
            var cacheItem = new SwitchToLinkedAccountCacheItem(
                targetTenantId,
                targetUserId,
                AbpSession.ImpersonatorTenantId,
                AbpSession.ImpersonatorUserId
                );

            //Create a random token and save to the cache
            var tokenId = Guid.NewGuid().ToString();
            await _cacheManager
                .GetSwitchToLinkedAccountCache()
                .SetAsync(tokenId, cacheItem, TimeSpan.FromMinutes(1));

            //Find tenancy name
            string tenancyName = null;
            if (targetTenantId.HasValue)
            {
                tenancyName = (await _tenantManager.GetByIdAsync(targetTenantId.Value)).TenancyName;
            }

            //Create target URL
            var targetUrl = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/SwitchToLinkedAccountSignIn?tokenId=" + tokenId;
            return Json(new AjaxResponse { TargetUrl = targetUrl });
        }

        #endregion

        #region Helpers

        private string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            if (defaultValueBuilder == null)
            {
                defaultValueBuilder = () => Url.Action("Index", "Application");
            }

            if (returnUrl.IsNullOrEmpty())
            {
                return defaultValueBuilder();
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return defaultValueBuilder();
        }

        #endregion

        #region Etc

        [AbpMvcAuthorize]
        public async Task<ActionResult> TestNotification(string message = "", string severity = "info")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            await _appNotifier.SendMessageAsync(
                AbpSession.ToUserIdentifier(),
                message,
                severity.ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>()
                );

            return Content("Sent notification: " + message);
        }

        #endregion
    }
}