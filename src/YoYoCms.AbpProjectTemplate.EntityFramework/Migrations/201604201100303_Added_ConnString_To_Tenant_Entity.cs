namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ConnString_To_Tenant_Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpTenants", "ConnectionString", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpTenants", "ConnectionString");
        }
    }
}
