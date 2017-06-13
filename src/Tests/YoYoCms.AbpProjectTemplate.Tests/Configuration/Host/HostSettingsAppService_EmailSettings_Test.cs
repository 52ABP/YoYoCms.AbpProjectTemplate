using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Net.Mail;
using YoYoCms.AbpProjectTemplate.Configuration.Host;
using Shouldly;
using Xunit;

namespace YoYoCms.AbpProjectTemplate.Tests.Configuration.Host
{
    public class HostSettingsAppService_EmailSettings_Test : AppTestBase
    {
        private readonly IHostSettingsAppService _hostSettingsAppService;
        private readonly ISettingManager _settingManager;

        public HostSettingsAppService_EmailSettings_Test()
        {
            _hostSettingsAppService = Resolve<IHostSettingsAppService>();
            _settingManager = Resolve<ISettingManager>();

            LoginAsHostAdmin();
            InitializeTestSettings();
        }

        private void InitializeTestSettings()
        {
            _settingManager.ChangeSettingForApplication(EmailSettingNames.DefaultFromAddress, "test@mydomain.com");
            _settingManager.ChangeSettingForApplication(EmailSettingNames.DefaultFromDisplayName, "");
            _settingManager.ChangeSettingForApplication(EmailSettingNames.Smtp.Host, "100.101.102.103");
            _settingManager.ChangeSettingForApplication(EmailSettingNames.Smtp.UserName, "myuser");
            _settingManager.ChangeSettingForApplication(EmailSettingNames.Smtp.Password, "123456");
            _settingManager.ChangeSettingForApplication(EmailSettingNames.Smtp.Domain, "mydomain");
            _settingManager.ChangeSettingForApplication(EmailSettingNames.Smtp.EnableSsl, "true");
            _settingManager.ChangeSettingForApplication(EmailSettingNames.Smtp.UseDefaultCredentials, "false");
        }

        [MultiTenantFact]
        public async Task Should_Change_Email_Settings()
        {
            //Get and check current settings

            //Act
            var settings = await _hostSettingsAppService.GetAllSettings();

            //Assert
            settings.Email.DefaultFromAddress.ShouldBe("test@mydomain.com");
            settings.Email.DefaultFromDisplayName.ShouldBe("");
            settings.Email.SmtpHost.ShouldBe("100.101.102.103");
            settings.Email.SmtpPort.ShouldBe(25); //this is the default value
            settings.Email.SmtpUserName.ShouldBe("myuser");
            settings.Email.SmtpPassword.ShouldBe("123456");
            settings.Email.SmtpDomain.ShouldBe("mydomain");
            settings.Email.SmtpEnableSsl.ShouldBe(true);
            settings.Email.SmtpUseDefaultCredentials.ShouldBe(false);

            //Change and save settings

            //Arrange
            settings.Email.DefaultFromDisplayName = "My daily mailing service";
            settings.Email.SmtpHost = "100.101.102.104";
            settings.Email.SmtpPort = 42;
            settings.Email.SmtpUserName = "changeduser";
            settings.Email.SmtpPassword = "654321";
            settings.Email.SmtpDomain = "changeddomain";
            settings.Email.SmtpEnableSsl = false;
            
            //Act
            await _hostSettingsAppService.UpdateAllSettings(settings);

            //Assert
            _settingManager.GetSettingValue(EmailSettingNames.DefaultFromAddress).ShouldBe("test@mydomain.com"); //not changed
            _settingManager.GetSettingValue(EmailSettingNames.DefaultFromDisplayName).ShouldBe("My daily mailing service");
            _settingManager.GetSettingValue(EmailSettingNames.Smtp.Host).ShouldBe("100.101.102.104");
            _settingManager.GetSettingValue<int>(EmailSettingNames.Smtp.Port).ShouldBe(42);
            _settingManager.GetSettingValue(EmailSettingNames.Smtp.UserName).ShouldBe("changeduser");
            _settingManager.GetSettingValue(EmailSettingNames.Smtp.Password).ShouldBe("654321");
            _settingManager.GetSettingValue(EmailSettingNames.Smtp.Domain).ShouldBe("changeddomain");
            _settingManager.GetSettingValue<bool>(EmailSettingNames.Smtp.EnableSsl).ShouldBe(false);
            _settingManager.GetSettingValue<bool>(EmailSettingNames.Smtp.UseDefaultCredentials).ShouldBe(false); //not changed
        }
    }
}
