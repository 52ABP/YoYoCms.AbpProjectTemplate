using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using YoYoCms.AbpProjectTemplate.EntityFramework;

namespace YoYoCms.AbpProjectTemplate.Migrations.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly AbpProjectTemplateDbContext _context;

        public DefaultSettingsCreator(AbpProjectTemplateDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 默认信息配置
        /// </summary>
        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@yoyocms.com");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "yoyocms.com mailer");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-CN");
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}