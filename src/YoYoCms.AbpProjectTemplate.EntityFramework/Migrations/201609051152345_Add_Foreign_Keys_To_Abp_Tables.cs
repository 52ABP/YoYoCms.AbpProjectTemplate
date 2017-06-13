namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Foreign_Keys_To_Abp_Tables : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("AbpUserOrganizationUnits", "UserId", "AbpUsers", "Id", cascadeDelete: true);
            AddForeignKey("AbpUserOrganizationUnits", "OrganizationUnitId", "AbpOrganizationUnits", "Id", cascadeDelete: true);
            AddForeignKey("AbpUserRoles", "RoleId", "AbpRoles", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("AbpUserRoles", "RoleId", "AbpRoles");
            DropForeignKey("AbpUserOrganizationUnits", "OrganizationUnitId", "AbpOrganizationUnits");
            DropForeignKey("AbpUserOrganizationUnits", "UserId", "AbpUsers");
        }
    }
}
