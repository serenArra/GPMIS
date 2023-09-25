using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace MFAE.Jobs.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var applicantStatuses = pages.CreateChildPermission(AppPermissions.Pages_ApplicantStatuses, L("ApplicantStatuses"));
            applicantStatuses.CreateChildPermission(AppPermissions.Pages_ApplicantStatuses_Create, L("CreateNewApplicantStatus"));
            applicantStatuses.CreateChildPermission(AppPermissions.Pages_ApplicantStatuses_Edit, L("EditApplicantStatus"));
            applicantStatuses.CreateChildPermission(AppPermissions.Pages_ApplicantStatuses_Delete, L("DeleteApplicantStatus"));

            var conversationRates = pages.CreateChildPermission(AppPermissions.Pages_ConversationRates, L("ConversationRates"));
            conversationRates.CreateChildPermission(AppPermissions.Pages_ConversationRates_Create, L("CreateNewConversationRate"));
            conversationRates.CreateChildPermission(AppPermissions.Pages_ConversationRates_Edit, L("EditConversationRate"));
            conversationRates.CreateChildPermission(AppPermissions.Pages_ConversationRates_Delete, L("DeleteConversationRate"));

            var applicantLanguages = pages.CreateChildPermission(AppPermissions.Pages_ApplicantLanguages, L("ApplicantLanguages"));
            applicantLanguages.CreateChildPermission(AppPermissions.Pages_ApplicantLanguages_Create, L("CreateNewApplicantLanguage"));
            applicantLanguages.CreateChildPermission(AppPermissions.Pages_ApplicantLanguages_Edit, L("EditApplicantLanguage"));
            applicantLanguages.CreateChildPermission(AppPermissions.Pages_ApplicantLanguages_Delete, L("DeleteApplicantLanguage"));

            var applicantTrainings = pages.CreateChildPermission(AppPermissions.Pages_ApplicantTrainings, L("ApplicantTrainings"));
            applicantTrainings.CreateChildPermission(AppPermissions.Pages_ApplicantTrainings_Create, L("CreateNewApplicantTraining"));
            applicantTrainings.CreateChildPermission(AppPermissions.Pages_ApplicantTrainings_Edit, L("EditApplicantTraining"));
            applicantTrainings.CreateChildPermission(AppPermissions.Pages_ApplicantTrainings_Delete, L("DeleteApplicantTraining"));

            var applicantStudies = pages.CreateChildPermission(AppPermissions.Pages_ApplicantStudies, L("ApplicantStudies"));
            applicantStudies.CreateChildPermission(AppPermissions.Pages_ApplicantStudies_Create, L("CreateNewApplicantStudy"));
            applicantStudies.CreateChildPermission(AppPermissions.Pages_ApplicantStudies_Edit, L("EditApplicantStudy"));
            applicantStudies.CreateChildPermission(AppPermissions.Pages_ApplicantStudies_Delete, L("DeleteApplicantStudy"));

            var maritalStatuses = pages.CreateChildPermission(AppPermissions.Pages_MaritalStatuses, L("MaritalStatuses"));
            maritalStatuses.CreateChildPermission(AppPermissions.Pages_MaritalStatuses_Create, L("CreateNewMaritalStatus"));
            maritalStatuses.CreateChildPermission(AppPermissions.Pages_MaritalStatuses_Edit, L("EditMaritalStatus"));
            maritalStatuses.CreateChildPermission(AppPermissions.Pages_MaritalStatuses_Delete, L("DeleteMaritalStatus"));

            var identificationTypes = pages.CreateChildPermission(AppPermissions.Pages_IdentificationTypes, L("IdentificationTypes"));
            identificationTypes.CreateChildPermission(AppPermissions.Pages_IdentificationTypes_Create, L("CreateNewIdentificationType"));
            identificationTypes.CreateChildPermission(AppPermissions.Pages_IdentificationTypes_Edit, L("EditIdentificationType"));
            identificationTypes.CreateChildPermission(AppPermissions.Pages_IdentificationTypes_Delete, L("DeleteIdentificationType"));

            var applicants = pages.CreateChildPermission(AppPermissions.Pages_Applicants, L("Applicants"));
            applicants.CreateChildPermission(AppPermissions.Pages_Applicants_Create, L("CreateNewApplicant"));
            applicants.CreateChildPermission(AppPermissions.Pages_Applicants_Edit, L("EditApplicant"));
            applicants.CreateChildPermission(AppPermissions.Pages_Applicants_Delete, L("DeleteApplicant"));

            var conversations = pages.CreateChildPermission(AppPermissions.Pages_Conversations, L("Conversations"));
            conversations.CreateChildPermission(AppPermissions.Pages_Conversations_Create, L("CreateNewConversation"));
            conversations.CreateChildPermission(AppPermissions.Pages_Conversations_Edit, L("EditConversation"));
            conversations.CreateChildPermission(AppPermissions.Pages_Conversations_Delete, L("DeleteConversation"));

            var graduationRates = pages.CreateChildPermission(AppPermissions.Pages_GraduationRates, L("GraduationRates"));
            graduationRates.CreateChildPermission(AppPermissions.Pages_GraduationRates_Create, L("CreateNewGraduationRate"));
            graduationRates.CreateChildPermission(AppPermissions.Pages_GraduationRates_Edit, L("EditGraduationRate"));
            graduationRates.CreateChildPermission(AppPermissions.Pages_GraduationRates_Delete, L("DeleteGraduationRate"));

            var specialtieses = pages.CreateChildPermission(AppPermissions.Pages_Specialtieses, L("Specialtieses"));
            specialtieses.CreateChildPermission(AppPermissions.Pages_Specialtieses_Create, L("CreateNewSpecialties"));
            specialtieses.CreateChildPermission(AppPermissions.Pages_Specialtieses_Edit, L("EditSpecialties"));
            specialtieses.CreateChildPermission(AppPermissions.Pages_Specialtieses_Delete, L("DeleteSpecialties"));

            var academicDegrees = pages.CreateChildPermission(AppPermissions.Pages_AcademicDegrees, L("AcademicDegrees"));
            academicDegrees.CreateChildPermission(AppPermissions.Pages_AcademicDegrees_Create, L("CreateNewAcademicDegree"));
            academicDegrees.CreateChildPermission(AppPermissions.Pages_AcademicDegrees_Edit, L("EditAcademicDegree"));
            academicDegrees.CreateChildPermission(AppPermissions.Pages_AcademicDegrees_Delete, L("DeleteAcademicDegree"));

            var appLanguages = pages.CreateChildPermission(AppPermissions.Pages_Languages, L("Languages"));
            appLanguages.CreateChildPermission(AppPermissions.Pages_Languages_Create, L("CreateNewLanguage"));
            appLanguages.CreateChildPermission(AppPermissions.Pages_Languages_Edit, L("EditLanguage"));
            appLanguages.CreateChildPermission(AppPermissions.Pages_Languages_Delete, L("DeleteLanguage"));

            var attachmentFiles = pages.CreateChildPermission(AppPermissions.Pages_AttachmentFiles, L("AttachmentFiles"));
            attachmentFiles.CreateChildPermission(AppPermissions.Pages_AttachmentFiles_Create, L("CreateNewAttachmentFile"));
            attachmentFiles.CreateChildPermission(AppPermissions.Pages_AttachmentFiles_Edit, L("EditAttachmentFile"));
            attachmentFiles.CreateChildPermission(AppPermissions.Pages_AttachmentFiles_Delete, L("DeleteAttachmentFile"));

            var attachmentTypes = pages.CreateChildPermission(AppPermissions.Pages_AttachmentTypes, L("AttachmentTypes"));
            attachmentTypes.CreateChildPermission(AppPermissions.Pages_AttachmentTypes_Create, L("CreateNewAttachmentType"));
            attachmentTypes.CreateChildPermission(AppPermissions.Pages_AttachmentTypes_Edit, L("EditAttachmentType"));
            attachmentTypes.CreateChildPermission(AppPermissions.Pages_AttachmentTypes_Delete, L("DeleteAttachmentType"));

            var attachmentTypeGroups = pages.CreateChildPermission(AppPermissions.Pages_AttachmentTypeGroups, L("AttachmentTypeGroups"));
            attachmentTypeGroups.CreateChildPermission(AppPermissions.Pages_AttachmentTypeGroups_Create, L("CreateNewAttachmentTypeGroup"));
            attachmentTypeGroups.CreateChildPermission(AppPermissions.Pages_AttachmentTypeGroups_Edit, L("EditAttachmentTypeGroup"));
            attachmentTypeGroups.CreateChildPermission(AppPermissions.Pages_AttachmentTypeGroups_Delete, L("DeleteAttachmentTypeGroup"));

            var attachmentEntityTypes = pages.CreateChildPermission(AppPermissions.Pages_AttachmentEntityTypes, L("AttachmentEntityTypes"));
            attachmentEntityTypes.CreateChildPermission(AppPermissions.Pages_AttachmentEntityTypes_Create, L("CreateNewAttachmentEntityType"));
            attachmentEntityTypes.CreateChildPermission(AppPermissions.Pages_AttachmentEntityTypes_Edit, L("EditAttachmentEntityType"));
            attachmentEntityTypes.CreateChildPermission(AppPermissions.Pages_AttachmentEntityTypes_Delete, L("DeleteAttachmentEntityType"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, JobsConsts.LocalizationSourceName);
        }
    }
}