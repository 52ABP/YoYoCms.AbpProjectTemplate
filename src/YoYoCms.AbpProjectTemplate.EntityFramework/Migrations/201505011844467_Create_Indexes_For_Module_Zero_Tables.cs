using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class Create_Indexes_For_Module_Zero_Tables : DbMigration
    {
        public override void Up()
        {
            DropIndex("AbpPermissions", new[] { "UserId" });
            DropIndex("AbpPermissions", new[] { "RoleId" });
            DropIndex("AbpRoles", new[] { "TenantId" });
            DropIndex("AbpSettings", new[] { "TenantId" });
            DropIndex("AbpSettings", new[] { "UserId" });
            DropIndex("AbpUserLogins", new[] { "UserId" });
            DropIndex("AbpUserRoles", new[] { "UserId" });
            DropIndex("AbpUsers", new[] { "TenantId" });

            CreateIndex("AbpAuditLogs", new[] { "TenantId", "ExecutionTime" });
            CreateIndex("AbpAuditLogs", new[] { "UserId", "ExecutionTime" });
            CreateIndex("AbpPermissions", new[] { "UserId", "Name" });
            CreateIndex("AbpPermissions", new[] { "RoleId", "Name" });
            CreateIndex("AbpRoles", new[] { "TenantId", "Name" });
            CreateIndex("AbpRoles", new[] { "IsDeleted", "TenantId", "Name" });
            CreateIndex("AbpSettings", new[] { "TenantId", "Name" });
            CreateIndex("AbpSettings", new[] { "UserId", "Name" });
            CreateIndex("AbpTenants", new[] { "TenancyName" });
            CreateIndex("AbpTenants", new[] { "IsDeleted", "TenancyName" });
            CreateIndex("AbpUserLogins", new[] { "UserId", "LoginProvider" });
            CreateIndex("AbpUserRoles", new[] { "UserId", "RoleId" });
            CreateIndex("AbpUserRoles", new[] { "RoleId" });
            CreateIndex("AbpUsers", new[] { "TenantId", "UserName" });
            CreateIndex("AbpUsers", new[] { "TenantId", "EmailAddress" });
            CreateIndex("AbpUsers", new[] { "IsDeleted", "TenantId", "UserName" });
            CreateIndex("AbpUsers", new[] { "IsDeleted", "TenantId", "EmailAddress" });
        }

        public override void Down()
        {
            DropIndex("AbpAuditLogs", new[] { "TenantId", "ExecutionTime" });
            DropIndex("AbpAuditLogs", new[] { "UserId", "ExecutionTime" });
            DropIndex("AbpPermissions", new[] { "UserId", "Name" });
            DropIndex("AbpPermissions", new[] { "RoleId", "Name" });
            DropIndex("AbpRoles", new[] { "TenantId", "Name" });
            DropIndex("AbpRoles", new[] { "IsDeleted", "TenantId", "Name" });
            DropIndex("AbpSettings", new[] { "TenantId", "Name" });
            DropIndex("AbpSettings", new[] { "UserId", "Name" });
            DropIndex("AbpTenants", new[] { "TenancyName" });
            DropIndex("AbpTenants", new[] { "IsDeleted", "TenancyName" });
            DropIndex("AbpUserLogins", new[] { "UserId", "LoginProvider" });
            DropIndex("AbpUserRoles", new[] { "UserId", "RoleId" });
            DropIndex("AbpUserRoles", new[] { "RoleId" });
            DropIndex("AbpUsers", new[] { "TenantId", "UserName" });
            DropIndex("AbpUsers", new[] { "TenantId", "EmailAddress" });
            DropIndex("AbpUsers", new[] { "IsDeleted", "TenantId", "UserName" });
            DropIndex("AbpUsers", new[] { "IsDeleted", "TenantId", "EmailAddress" });

            CreateIndex("AbpPermissions", new[] { "UserId" });
            CreateIndex("AbpPermissions", new[] { "RoleId" });
            CreateIndex("AbpRoles", new[] { "TenantId" });
            CreateIndex("AbpSettings", new[] { "TenantId" });
            CreateIndex("AbpSettings", new[] { "UserId" });
            CreateIndex("AbpUserLogins", new[] { "UserId" });
            CreateIndex("AbpUserRoles", new[] { "UserId" });
            CreateIndex("AbpUsers", new[] { "TenantId" });
        }
    }
}
