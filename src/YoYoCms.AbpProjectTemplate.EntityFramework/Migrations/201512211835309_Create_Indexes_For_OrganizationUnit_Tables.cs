namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Indexes_For_OrganizationUnit_Tables : DbMigration
    {
        public override void Up()
        {
            CreateIndex("AbpOrganizationUnits", new[] { "TenantId", "ParentId" });
            CreateIndex("AbpOrganizationUnits", new[] { "TenantId", "Code" });

            CreateIndex("AbpUserOrganizationUnits", new[] { "TenantId", "UserId" });
            CreateIndex("AbpUserOrganizationUnits", new[] { "TenantId", "OrganizationUnitId" });
            CreateIndex("AbpUserOrganizationUnits", new[] { "UserId" });
            CreateIndex("AbpUserOrganizationUnits", new[] { "OrganizationUnitId" });
        }
        
        public override void Down()
        {
            DropIndex("AbpUserOrganizationUnits", new[] { "OrganizationUnitId" });
            DropIndex("AbpUserOrganizationUnits", new[] { "UserId" });
            DropIndex("AbpUserOrganizationUnits", new[] { "TenantId", "OrganizationUnitId" });
            DropIndex("AbpUserOrganizationUnits", new[] { "TenantId", "UserId" });

            DropIndex("AbpOrganizationUnits", new[] { "TenantId", "Code" });
            DropIndex("AbpOrganizationUnits", new[] { "TenantId", "ParentId" });
        }
    }
}
