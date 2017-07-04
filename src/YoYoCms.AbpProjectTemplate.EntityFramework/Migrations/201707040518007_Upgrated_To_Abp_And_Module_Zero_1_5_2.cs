namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Upgrated_To_Abp_And_Module_Zero_1_5_2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AbpAuditLogs", newName: "AuditLogs");
            RenameTable(name: "dbo.AbpBackgroundJobs", newName: "BackgroundJobs");
            RenameTable(name: "dbo.AbpFeatures", newName: "Features");
            RenameTable(name: "dbo.AbpEditions", newName: "Editions");
            RenameTable(name: "dbo.AbpLanguages", newName: "Languages");
            RenameTable(name: "dbo.AbpLanguageTexts", newName: "LanguageTexts");
            RenameTable(name: "dbo.AbpNotifications", newName: "Notifications");
            RenameTable(name: "dbo.AbpNotificationSubscriptions", newName: "NotificationSubscriptions");
            RenameTable(name: "dbo.AbpOrganizationUnits", newName: "OrganizationUnits");
            RenameTable(name: "dbo.AbpPermissions", newName: "Permissions");
            RenameTable(name: "dbo.AbpRoles", newName: "Roles");
            RenameTable(name: "dbo.AbpUsers", newName: "Users");
            RenameTable(name: "dbo.AbpUserClaims", newName: "UserClaims");
            RenameTable(name: "dbo.AbpUserLogins", newName: "UserLogins");
            RenameTable(name: "dbo.AbpUserRoles", newName: "UserRoles");
            RenameTable(name: "dbo.AbpSettings", newName: "Settings");
            RenameTable(name: "dbo.AbpTenantNotifications", newName: "TenantNotifications");
            RenameTable(name: "dbo.AbpTenants", newName: "Tenants");
            RenameTable(name: "dbo.AbpUserAccounts", newName: "UserAccounts");
            RenameTable(name: "dbo.AbpUserLoginAttempts", newName: "UserLoginAttempts");
            RenameTable(name: "dbo.AbpUserNotifications", newName: "UserNotifications");
            RenameTable(name: "dbo.AbpUserOrganizationUnits", newName: "UserOrganizationUnits");
            MoveTable(name: "dbo.AuditLogs", newSchema: "ABP");
            MoveTable(name: "dbo.BackgroundJobs", newSchema: "ABP");
            MoveTable(name: "dbo.Features", newSchema: "ABP");
            MoveTable(name: "dbo.Editions", newSchema: "ABP");
            MoveTable(name: "dbo.Languages", newSchema: "ABP");
            MoveTable(name: "dbo.LanguageTexts", newSchema: "ABP");
            MoveTable(name: "dbo.Notifications", newSchema: "ABP");
            MoveTable(name: "dbo.NotificationSubscriptions", newSchema: "ABP");
            MoveTable(name: "dbo.OrganizationUnits", newSchema: "ABP");
            MoveTable(name: "dbo.Permissions", newSchema: "ABP");
            MoveTable(name: "dbo.Roles", newSchema: "ABP");
            MoveTable(name: "dbo.Users", newSchema: "ABP");
            MoveTable(name: "dbo.UserClaims", newSchema: "ABP");
            MoveTable(name: "dbo.UserLogins", newSchema: "ABP");
            MoveTable(name: "dbo.UserRoles", newSchema: "ABP");
            MoveTable(name: "dbo.Settings", newSchema: "ABP");
            MoveTable(name: "dbo.TenantNotifications", newSchema: "ABP");
            MoveTable(name: "dbo.Tenants", newSchema: "ABP");
            MoveTable(name: "dbo.UserAccounts", newSchema: "ABP");
            MoveTable(name: "dbo.UserLoginAttempts", newSchema: "ABP");
            MoveTable(name: "dbo.UserNotifications", newSchema: "ABP");
            MoveTable(name: "dbo.UserOrganizationUnits", newSchema: "ABP");
        }
        
        public override void Down()
        {
            MoveTable(name: "ABP.UserOrganizationUnits", newSchema: "dbo");
            MoveTable(name: "ABP.UserNotifications", newSchema: "dbo");
            MoveTable(name: "ABP.UserLoginAttempts", newSchema: "dbo");
            MoveTable(name: "ABP.UserAccounts", newSchema: "dbo");
            MoveTable(name: "ABP.Tenants", newSchema: "dbo");
            MoveTable(name: "ABP.TenantNotifications", newSchema: "dbo");
            MoveTable(name: "ABP.Settings", newSchema: "dbo");
            MoveTable(name: "ABP.UserRoles", newSchema: "dbo");
            MoveTable(name: "ABP.UserLogins", newSchema: "dbo");
            MoveTable(name: "ABP.UserClaims", newSchema: "dbo");
            MoveTable(name: "ABP.Users", newSchema: "dbo");
            MoveTable(name: "ABP.Roles", newSchema: "dbo");
            MoveTable(name: "ABP.Permissions", newSchema: "dbo");
            MoveTable(name: "ABP.OrganizationUnits", newSchema: "dbo");
            MoveTable(name: "ABP.NotificationSubscriptions", newSchema: "dbo");
            MoveTable(name: "ABP.Notifications", newSchema: "dbo");
            MoveTable(name: "ABP.LanguageTexts", newSchema: "dbo");
            MoveTable(name: "ABP.Languages", newSchema: "dbo");
            MoveTable(name: "ABP.Editions", newSchema: "dbo");
            MoveTable(name: "ABP.Features", newSchema: "dbo");
            MoveTable(name: "ABP.BackgroundJobs", newSchema: "dbo");
            MoveTable(name: "ABP.AuditLogs", newSchema: "dbo");
            RenameTable(name: "dbo.UserOrganizationUnits", newName: "AbpUserOrganizationUnits");
            RenameTable(name: "dbo.UserNotifications", newName: "AbpUserNotifications");
            RenameTable(name: "dbo.UserLoginAttempts", newName: "AbpUserLoginAttempts");
            RenameTable(name: "dbo.UserAccounts", newName: "AbpUserAccounts");
            RenameTable(name: "dbo.Tenants", newName: "AbpTenants");
            RenameTable(name: "dbo.TenantNotifications", newName: "AbpTenantNotifications");
            RenameTable(name: "dbo.Settings", newName: "AbpSettings");
            RenameTable(name: "dbo.UserRoles", newName: "AbpUserRoles");
            RenameTable(name: "dbo.UserLogins", newName: "AbpUserLogins");
            RenameTable(name: "dbo.UserClaims", newName: "AbpUserClaims");
            RenameTable(name: "dbo.Users", newName: "AbpUsers");
            RenameTable(name: "dbo.Roles", newName: "AbpRoles");
            RenameTable(name: "dbo.Permissions", newName: "AbpPermissions");
            RenameTable(name: "dbo.OrganizationUnits", newName: "AbpOrganizationUnits");
            RenameTable(name: "dbo.NotificationSubscriptions", newName: "AbpNotificationSubscriptions");
            RenameTable(name: "dbo.Notifications", newName: "AbpNotifications");
            RenameTable(name: "dbo.LanguageTexts", newName: "AbpLanguageTexts");
            RenameTable(name: "dbo.Languages", newName: "AbpLanguages");
            RenameTable(name: "dbo.Editions", newName: "AbpEditions");
            RenameTable(name: "dbo.Features", newName: "AbpFeatures");
            RenameTable(name: "dbo.BackgroundJobs", newName: "AbpBackgroundJobs");
            RenameTable(name: "dbo.AuditLogs", newName: "AbpAuditLogs");
        }
    }
}
