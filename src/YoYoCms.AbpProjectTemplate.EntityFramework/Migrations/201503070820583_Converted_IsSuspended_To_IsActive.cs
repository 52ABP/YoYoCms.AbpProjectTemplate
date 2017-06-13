using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class Converted_IsSuspended_To_IsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.AbpTenants", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            DropColumn("dbo.AbpUsers", "IsSuspended");
            DropColumn("dbo.AbpTenants", "IsSuspended");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbpTenants", "IsSuspended", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.AbpUsers", "IsSuspended", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn("dbo.AbpTenants", "IsActive");
            DropColumn("dbo.AbpUsers", "IsActive");
        }
    }
}
