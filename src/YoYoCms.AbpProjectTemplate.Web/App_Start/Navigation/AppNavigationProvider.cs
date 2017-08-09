using Abp.Application.Navigation;
using Abp.Localization;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Authorization;

namespace YoYoCms.AbpProjectTemplate.Web.Navigation
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class AppNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var smsMessagelog = new MenuItemDefinition(
                 SmsMessagelogAppPermissions.SmsMessagelog,
                 L("SmsMessagelog"),
                 url: "smsMessagelogs",
                 icon: "icon-grid",
                  requiredPermissionName: SmsMessagelogAppPermissions.SmsMessagelog
                 );
            context.Manager.MainMenu
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Tenants,
                    L("Tenants"),
                    url: "host.tenants",
                    icon: "icon-globe",
                    requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Editions,
                    L("Editions"),
                    url: "host.editions",
                    icon: "icon-grid",
                    requiredPermissionName: AppPermissions.Pages_Editions
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Tenant.Dashboard,
                    L("Dashboard"),
                    url: "tenant.dashboard",
                    icon: "icon-home",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Dashboard
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Administration,
                    L("Administration"),
                    icon: "icon-wrench"
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OrganizationUnits,
                        L("OrganizationUnits"),
                        url: "organizationUnits",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Roles,
                        L("Roles"),
                        url: "roles",
                        icon: "icon-briefcase",
                        requiredPermissionName: AppPermissions.Pages_Administration_Roles
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Users,
                        L("Users"),
                        url: "users",
                        icon: "icon-users",
                        requiredPermissionName: AppPermissions.Pages_Administration_Users
                        )
                    ).AddItem(smsMessagelog).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Languages,
                        L("Languages"),
                        url: "languages",
                        icon: "icon-flag",
                        requiredPermissionName: AppPermissions.Pages_Administration_Languages
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.AuditLogs,
                        L("AuditLogs"),
                        url: "auditLogs",
                        icon: "icon-lock",
                        requiredPermissionName: AppPermissions.Pages_Administration_AuditLogs
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Maintenance,
                        L("Maintenance"),
                        url: "host.maintenance",
                        icon: "icon-wrench",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Maintenance
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Settings,
                        L("Settings"),
                        url: "host.settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Settings
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Tenant.Settings,
                        L("Settings"),
                        url: "tenant.settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Tenant_Settings
                        )
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpProjectTemplateConsts.LocalizationSourceName);
        }
    }
}
