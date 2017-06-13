namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Indexes_For_Language_Tables : DbMigration
    {
        public override void Up()
        {
            CreateIndex("AbpLanguages", new[] { "TenantId", "Name" });

            CreateIndex("AbpLanguageTexts", new[] { "TenantId", "LanguageName", "Source", "Key" });
        }
        
        public override void Down()
        {
            DropIndex("AbpLanguageTexts", new[] { "TenantId", "LanguageName", "Source", "Key" });

            DropIndex("AbpLanguages", new[] { "TenantId", "Name" });
        }
    }
}
