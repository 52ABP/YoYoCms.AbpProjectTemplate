using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class Added_Tenant_Property_Annotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AbpTenants", "TenancyName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.AbpTenants", "Name", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AbpTenants", "Name", c => c.String());
            AlterColumn("dbo.AbpTenants", "TenancyName", c => c.String());
        }
    }
}
