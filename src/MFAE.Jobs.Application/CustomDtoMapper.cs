using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.XRoad;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Location;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Attachments;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Extensions;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using MFAE.Jobs.Auditing.Dto;
using MFAE.Jobs.Authorization.Accounts.Dto;
using MFAE.Jobs.Authorization.Delegation;
using MFAE.Jobs.Authorization.Permissions.Dto;
using MFAE.Jobs.Authorization.Roles;
using MFAE.Jobs.Authorization.Roles.Dto;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Authorization.Users.Delegation.Dto;
using MFAE.Jobs.Authorization.Users.Dto;
using MFAE.Jobs.Authorization.Users.Importing.Dto;
using MFAE.Jobs.Authorization.Users.Profile.Dto;
using MFAE.Jobs.Chat;
using MFAE.Jobs.Chat.Dto;
using MFAE.Jobs.DynamicEntityProperties.Dto;
using MFAE.Jobs.Editions;
using MFAE.Jobs.Editions.Dto;
using MFAE.Jobs.Friendships;
using MFAE.Jobs.Friendships.Cache;
using MFAE.Jobs.Friendships.Dto;
using MFAE.Jobs.Localization.Dto;
using MFAE.Jobs.MultiTenancy;
using MFAE.Jobs.MultiTenancy.Dto;
using MFAE.Jobs.MultiTenancy.HostDashboard.Dto;
using MFAE.Jobs.MultiTenancy.Payments;
using MFAE.Jobs.MultiTenancy.Payments.Dto;
using MFAE.Jobs.Notifications.Dto;
using MFAE.Jobs.Organizations.Dto;
using MFAE.Jobs.Sessions.Dto;
using MFAE.Jobs.WebHooks.Dto;

namespace MFAE.Jobs
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditJobAdvertisementDto, JobAdvertisement>().ReverseMap();
            configuration.CreateMap<JobAdvertisementDto, JobAdvertisement>().ReverseMap();
            configuration.CreateMap<CreateOrEditXRoadServiceErrorDto, XRoadServiceError>().ReverseMap();
            configuration.CreateMap<XRoadServiceErrorDto, XRoadServiceError>().ReverseMap();
            configuration.CreateMap<CreateOrEditXRoadServiceAttributeMappingDto, XRoadServiceAttributeMapping>().ReverseMap();
            configuration.CreateMap<XRoadServiceAttributeMappingDto, XRoadServiceAttributeMapping>().ReverseMap();
            configuration.CreateMap<CreateOrEditXRoadServiceAttributeDto, XRoadServiceAttribute>().ReverseMap();
            configuration.CreateMap<XRoadServiceAttributeDto, XRoadServiceAttribute>().ReverseMap();
            configuration.CreateMap<CreateOrEditXRoadServiceDto, XRoadService>().ReverseMap();
            configuration.CreateMap<XRoadServiceDto, XRoadService>().ReverseMap();
            configuration.CreateMap<CreateOrEditXRoadMappingDto, XRoadMapping>().ReverseMap();
            configuration.CreateMap<XRoadMappingDto, XRoadMapping>().ReverseMap();
            configuration.CreateMap<CreateOrEditLocalityDto, Locality>().ReverseMap();
            configuration.CreateMap<LocalityDto, Locality>().ReverseMap();
            configuration.CreateMap<CreateOrEditGovernorateDto, Governorate>().ReverseMap();
            configuration.CreateMap<GovernorateDto, Governorate>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            configuration.CreateMap<CreateOrEditApplicantStatusDto, ApplicantStatus>().ReverseMap();
            configuration.CreateMap<ApplicantStatusDto, ApplicantStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditConversationRateDto, ConversationRate>().ReverseMap();
            configuration.CreateMap<ConversationRateDto, ConversationRate>().ReverseMap();
            configuration.CreateMap<CreateOrEditApplicantLanguageDto, ApplicantLanguage>().ReverseMap();
            configuration.CreateMap<ApplicantLanguageDto, ApplicantLanguage>().ReverseMap();
            configuration.CreateMap<CreateOrEditApplicantTrainingDto, ApplicantTraining>().ReverseMap();
            configuration.CreateMap<ApplicantTrainingDto, ApplicantTraining>().ReverseMap();
            configuration.CreateMap<CreateOrEditApplicantStudyDto, ApplicantStudy>().ReverseMap();
            configuration.CreateMap<ApplicantStudyDto, ApplicantStudy>().ReverseMap();
            configuration.CreateMap<CreateOrEditMaritalStatusDto, MaritalStatus>().ReverseMap();
            configuration.CreateMap<MaritalStatusDto, MaritalStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditIdentificationTypeDto, IdentificationType>().ReverseMap();
            configuration.CreateMap<IdentificationTypeDto, IdentificationType>().ReverseMap();
            configuration.CreateMap<CreateOrEditApplicantDto, Applicant>().ReverseMap();
            configuration.CreateMap<CreateOrEditApplicantDto, ApplicantDto>().ReverseMap();
            configuration.CreateMap<ApplicantDto, Applicant>().ReverseMap();
            configuration.CreateMap<CreateOrEditConversationDto, Conversation>().ReverseMap();
            configuration.CreateMap<ConversationDto, Conversation>().ReverseMap();
            configuration.CreateMap<CreateOrEditGraduationRateDto, GraduationRate>().ReverseMap();
            configuration.CreateMap<GraduationRateDto, GraduationRate>().ReverseMap();
            configuration.CreateMap<CreateOrEditSpecialtiesDto, Specialties>().ReverseMap();
            configuration.CreateMap<SpecialtiesDto, Specialties>().ReverseMap();
            configuration.CreateMap<CreateOrEditAcademicDegreeDto, AcademicDegree>().ReverseMap();
            configuration.CreateMap<AcademicDegreeDto, AcademicDegree>().ReverseMap();
            configuration.CreateMap<CreateOrEditAppLanguageDto, AppLanguage>().ReverseMap();
            configuration.CreateMap<AppLanguageDto, AppLanguage>().ReverseMap();
            configuration.CreateMap<CreateOrEditAttachmentFileDto, AttachmentFile>().ReverseMap();
            configuration.CreateMap<AttachmentFileDto, AttachmentFile>().ReverseMap();
            configuration.CreateMap<CreateOrEditAttachmentTypeDto, AttachmentType>().ReverseMap();
            configuration.CreateMap<AttachmentTypeDto, AttachmentType>().ReverseMap();
            configuration.CreateMap<CreateOrEditAttachmentTypeGroupDto, AttachmentTypeGroup>().ReverseMap();
            configuration.CreateMap<AttachmentTypeGroupDto, AttachmentTypeGroup>().ReverseMap();
            configuration.CreateMap<CreateOrEditAttachmentEntityTypeDto, AttachmentEntityType>().ReverseMap();
            configuration.CreateMap<AttachmentEntityTypeDto, AttachmentEntityType>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}