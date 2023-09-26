namespace MFAE.Jobs.Web.Areas.App.Startup
{
    public class AppPageNames
    {
        public static class Common
        {
            public const string Localities = "Location.Localities";
            public const string Governorates = "Location.Governorates";
            public const string Countries = "Location.Countries";
            public const string ApplicantStatuses = "ApplicationForm.ApplicantStatuses";
            public const string ConversationRates = "ApplicationForm.ConversationRates";
            public const string ApplicantLanguages = "ApplicationForm.ApplicantLanguages";
            public const string ApplicantTrainings = "ApplicationForm.ApplicantTrainings";
            public const string ApplicantStudies = "ApplicationForm.ApplicantStudies";
            public const string MaritalStatuses = "ApplicationForm.MaritalStatuses";
            public const string IdentificationTypes = "ApplicationForm.IdentificationTypes";
            public const string Applicants = "ApplicationForm.Applicants";
            public const string Conversations = "ApplicationForm.Conversations";
            public const string GraduationRates = "ApplicationForm.GraduationRates";
            public const string Specialtieses = "ApplicationForm.Specialtieses";
            public const string AcademicDegrees = "ApplicationForm.AcademicDegrees";
            public const string AppLanguages = "ApplicationForm.Languages";
            public const string AttachmentFiles = "Attachments.AttachmentFiles";
            public const string AttachmentTypes = "Attachments.AttachmentTypes";
            public const string AttachmentTypeGroups = "Attachments.AttachmentTypeGroups";
            public const string AttachmentEntityTypes = "Attachments.AttachmentEntityTypes";
            public const string Administration = "Administration";
            public const string Roles = "Administration.Roles";
            public const string Users = "Administration.Users";
            public const string AuditLogs = "Administration.AuditLogs";
            public const string OrganizationUnits = "Administration.OrganizationUnits";
            public const string Languages = "Administration.Languages";
            public const string DemoUiComponents = "Administration.DemoUiComponents";
            public const string UiCustomization = "Administration.UiCustomization";
            public const string WebhookSubscriptions = "Administration.WebhookSubscriptions";
            public const string DynamicProperties = "Administration.DynamicProperties";
            public const string DynamicEntityProperties = "Administration.DynamicEntityProperties";
            public const string Notifications = "Administration.Notifications";
            public const string Notifications_Inbox = "Administration.Notifications.Inbox";
            public const string Notifications_MassNotifications = "Administration.Notifications.MassNotifications";
        }

        public static class Host
        {
            public const string Tenants = "Tenants";
            public const string Editions = "Editions";
            public const string Maintenance = "Administration.Maintenance";
            public const string Settings = "Administration.Settings.Host";
            public const string Dashboard = "Dashboard";
        }

        public static class Tenant
        {
            public const string Dashboard = "Dashboard.Tenant";
            public const string Settings = "Administration.Settings.Tenant";
            public const string SubscriptionManagement = "Administration.SubscriptionManagement.Tenant";
        }
    }
}