namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Upgrated_To_Module_Zero_0_8_4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AbpUserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
            AlterColumn("dbo.AbpUserLoginAttempts", "UserNameOrEmailAddress", c => c.String(maxLength: 255));
            CreateIndex("dbo.AbpUserLoginAttempts", new[] { "UserId", "TenantId" });
            CreateIndex("dbo.AbpUserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
        }
        
        public override void Down()
        {
            DropIndex("dbo.AbpUserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
            DropIndex("dbo.AbpUserLoginAttempts", new[] { "UserId", "TenantId" });
            AlterColumn("dbo.AbpUserLoginAttempts", "UserNameOrEmailAddress", c => c.String(maxLength: 256));
            CreateIndex("dbo.AbpUserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
        }
    }
}
