using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class ModuleZero_Upgrade_For_ISuspendable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "IsSuspended", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.AbpTenants", "IsSuspended", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AbpTenants", "IsSuspended");
            DropColumn("dbo.AbpUsers", "IsSuspended");
        }
    }
}
