using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using MFAE.Jobs.Authorization;

namespace MFAE.Jobs.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "App/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.JobAdvertisements,
                        L("JobAdvertisements"),
                        url: "App/JobAdvertisements",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_JobAdvertisements)
                    )
                )
               .AddItem(new MenuItemDefinition(
                           AppPageNames.Common.Applicants,
                           L("Applicants"),
                           url: "App/Applicants",
                           icon: "flaticon-more",
                           permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Applicants)
                       )
                   )

               .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("LocationModel"),
                        icon: "flaticon-pin"
                    )
                   .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Localities,
                            L("Localities"),
                            url: "App/Localities",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Localities)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Governorates,
                            L("Governorates"),
                            url: "App/Governorates",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Governorates)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Countries,
                            L("Countries"),
                            url: "App/Countries",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Countries)
                        )
                    )
                )

               .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("ApplicationModel"),
                        icon: "flaticon-pin"
                    )
               .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.InformationBankPage,
                        L("InformationBank"),
                        url: "App/XRoadServices/ViewInformationBankPage",
                        icon: "flaticon-notes",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_XRoadServices_InformationBank)
                    )
                )
                   .AddItem(new MenuItemDefinition(
                           AppPageNames.Common.AcademicDegrees,
                           L("AcademicDegrees"),
                           url: "App/AcademicDegrees",
                           icon: "flaticon-more",
                           permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AcademicDegrees)
                       )
                    )
                   .AddItem(new MenuItemDefinition(
                          AppPageNames.Common.ApplicantStatuses,
                          L("ApplicantStatuses"),
                          url: "App/ApplicantStatuses",
                          icon: "flaticon-more",
                          permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ApplicantStatuses)
                      )
                    )
                   .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ApplicantLanguages,
                            L("ApplicantLanguages"),
                            url: "App/ApplicantLanguages",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ApplicantLanguages)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ApplicantTrainings,
                            L("ApplicantTrainings"),
                            url: "App/ApplicantTrainings",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ApplicantTrainings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ApplicantStudies,
                            L("ApplicantStudies"),
                            url: "App/ApplicantStudies",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ApplicantStudies)
                        )
                    )
                   .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.ConversationRates,
                        L("ConversationRates"),
                        url: "App/ConversationRates",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ConversationRates)
                      )
                   )
                   .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Conversations,
                        L("Conversations"),
                        url: "App/Conversations",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Conversations)
                    )
                   )
                  .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.GraduationRates,
                        L("GraduationRates"),
                        url: "App/GraduationRates",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_GraduationRates)
                    )
                   )
                  .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.IdentificationTypes,
                        L("IdentificationTypes"),
                        url: "App/IdentificationTypes",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_IdentificationTypes)
                    )
                   )
                  .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.MaritalStatuses,
                        L("MaritalStatuses"),
                        url: "App/MaritalStatuses",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MaritalStatuses)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Specialtieses,
                        L("Specialtieses"),
                        url: "App/Specialtieses",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Specialtieses)
                    )
                  )
                )

                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Tenants,
                        L("Tenants"),
                        url: "App/Tenants",
                        icon: "flaticon-list-3",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Editions,
                        L("Editions"),
                        url: "App/Editions",
                        icon: "flaticon-app",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "App/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Attachments"),
                        icon: "flaticon-attachment"
                    ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.AttachmentFiles,
                        L("AttachmentFiles"),
                        url: "App/AttachmentFiles",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AttachmentFiles)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.AttachmentTypes,
                        L("AttachmentTypes"),
                        url: "App/AttachmentTypes",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AttachmentTypes)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.AttachmentTypeGroups,
                        L("AttachmentTypeGroups"),
                        url: "App/AttachmentTypeGroups",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AttachmentTypeGroups)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.AttachmentEntityTypes,
                        L("AttachmentEntityTypes"),
                        url: "App/AttachmentEntityTypes",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AttachmentEntityTypes)
                    )
                )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                       AppPageNames.Common.XRoadServices,
                       L("XRoadServices"),
                       url: "App/XRoadServices",
                       icon: "flaticon-exclamation-2",
                       permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_XRoadServices)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "App/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_UiCustomization)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.WebhookSubscriptions,
                            L("WebhookSubscriptions"),
                            url: "App/WebhookSubscription",
                            icon: "flaticon2-world",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_WebhookSubscription)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.DynamicProperties,
                            L("DynamicProperties"),
                            url: "App/DynamicProperty",
                            icon: "flaticon-interface-8",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_DynamicProperties)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Notifications,
                            L("Notifications"),
                            icon: "flaticon-alarm"
                        ).AddItem(new MenuItemDefinition(
                                AppPageNames.Common.Notifications_Inbox,
                                L("Inbox"),
                                url: "App/Notifications",
                                icon: "flaticon-mail-1"
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.Notifications_MassNotifications,
                                L("MassNotifications"),
                                url: "App/Notifications/MassNotifications",
                                icon: "flaticon-paper-plane",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_MassNotification)
                            )
                        )
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.DemoUiComponents,
                        L("DemoUiComponents"),
                        url: "App/DemoUiComponents",
                        icon: "flaticon-shapes",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DemoUiComponents)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, JobsConsts.LocalizationSourceName);
        }
    }
}