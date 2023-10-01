using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using MFAE.Jobs.Authorization.Roles;
using MFAE.Jobs.Configuration;
using MFAE.Jobs.Debugging;
using MFAE.Jobs.MultiTenancy;
using MFAE.Jobs.Notifications;
using Abp.Authorization;
using System.Globalization;
using Abp.Domain.Repositories;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.Authorization.Users.Dto;
using Microsoft.EntityFrameworkCore;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.ApplicationForm.Enums;
using MFAE.Jobs.Location;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.XRoad;
using System.Dynamic;
using MFAE.Jobs.XrodService;

namespace MFAE.Jobs.Authorization.Users
{
    public class UserRegistrationManager : JobsDomainServiceBase
    {
        public IAbpSession AbpSession { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserPolicy _userPolicy;
        private readonly IRepository<IdentificationType> _lookup_identificationTypeRepository;
        private readonly IXRoadServicesAppService _xRoadServicesAppService;
        private readonly IRepository<MaritalStatus> _lookup_maritalStatusRepository;
        private readonly IRepository<Country> _lookup_countryRepository;
        private readonly IRepository<Governorate> _lookup_governorateRepository;
        private readonly IRepository<Locality> _lookup_localityRepository;
        private readonly IRepository<Applicant, long> _applicantRepository;


        public UserRegistrationManager(
            IRepository<IdentificationType> lookup_identificationTypeRepository,
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUserEmailer userEmailer,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IUserPolicy userPolicy,
            IRepository<Applicant, long> applicantRepository,
            IRepository<Locality> lookup_localityRepository,
            IRepository<Governorate> lookup_governorateRepository,
            IRepository<Country> lookup_countryRepository,
            IRepository<MaritalStatus> lookup_maritalStatusRepository,
            IXRoadServicesAppService xRoadServicesAppService)
        {
            _lookup_identificationTypeRepository = lookup_identificationTypeRepository;
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _userPolicy = userPolicy;
            _xRoadServicesAppService = xRoadServicesAppService;
            _lookup_maritalStatusRepository = lookup_maritalStatusRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_localityRepository = lookup_localityRepository;
            _lookup_governorateRepository = lookup_governorateRepository;


            AbpSession = NullAbpSession.Instance;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _applicantRepository = applicantRepository;
        }

        public async Task<User> RegisterAsync(int? identificationTypeId, string documentNo , string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed, string emailActivationLink)
        {
            CheckForTenant();
            CheckSelfRegistrationIsEnabled();

            var tenant = await GetActiveTenantAsync();
            var isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault);

            await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

            var user = new User
            {
                IdentificationTypeId = identificationTypeId,
                DocumentNo = documentNo,
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = isNewRegisteredUserActiveByDefault,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault));
            foreach (var defaultRole in defaultRoles)
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            await CurrentUnitOfWork.SaveChangesAsync();

            if (!user.IsEmailConfirmed)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
            }

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }

     
        public async Task<List<UserIdentificationTypeLookupTableDto>> GetAllIdentificationTypeForTableDropdown()
        {
            return await _lookup_identificationTypeRepository.GetAll().OrderBy(e => e.Id)
               .Select(identificationType => new UserIdentificationTypeLookupTableDto
               {
                   Id = identificationType.Id,
                   NameAr = identificationType == null || identificationType.NameAr == null ? "" : identificationType.NameAr.ToString(),
                   NameEn = identificationType == null || identificationType.NameEn == null ? "" : identificationType.NameEn.ToString(),
                   IsDefault = identificationType.IsDefault
               }).ToListAsync();
        }
        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool UseCaptchaOnRegistration()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<GetApplicantForEditOutput> GetApplicantInfo(FetchPersonDto input)
        {
            var person = new Applicant();
            var birthCountry = new Country();
            var birthGovernorate = new Governorate();
            var birthLocality = new Locality();


            if (input.IdentificationDocumentNoTypeId == PersonNationalityConsts.palestinianIdentityTypeID)
            {
                var obj = await _xRoadServicesAppService.GetDynamic(XRoadServiceConsts.MOICitizensListWithCodes, "<CardID>" + input.IdentificationDocumentNoId + "</CardID>");
                ExpandoObject propertyNames = (ExpandoObject)obj.Where(v => v.Key == "response").Select(x => x.Value).FirstOrDefault();
                ExpandoObject view = (ExpandoObject)propertyNames.Where(v => v.Key == "citizenWithCode").Select(x => x.Value).FirstOrDefault();
                var propertyList = (IDictionary<String, Object>)view;
                person = new Applicant();
                person.DocumentNo = input.IdentificationDocumentNoId;
                person.FirstName = propertyList["FirstName"].ToString();
                person.FatherName = propertyList["FatherName"].ToString();
                person.GrandFatherName = propertyList["GrandFatherName"].ToString();
                person.FamilyName = propertyList["FamilyName"].ToString();
                person.FirstNameEn = propertyList["FirstName_EN"]?.ToString();
                person.FatherNameEn = propertyList["FatherName_EN"]?.ToString();
                person.GrandFatherNameEn = propertyList["GrandFatherName_EN"]?.ToString();
                person.FamilyNameEn = propertyList["FamilyName_EN"]?.ToString();
                person.Gender = (Gender)int.Parse(propertyList["SexId"].ToString());
                person.MaritalStatusId = await _lookup_maritalStatusRepository.GetAll().Where(e => e.NameAr == propertyList["MaritalStatusName"].ToString()).Select(e => e.Id).FirstOrDefaultAsync();

                person.BirthDate = DateTime.Parse(propertyList["BirthDate"].ToString(), null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                birthCountry = await _lookup_countryRepository.GetAll().Where(e => e.NameAr.Contains(propertyList["BirthCountryName"].ToString())).FirstOrDefaultAsync();


                if (birthCountry != null)
                {
                    birthGovernorate = await _lookup_governorateRepository.GetAll().Where(e => e.NameAr.Contains(propertyList["BirthCityName"].ToString())).FirstOrDefaultAsync();

                    if (birthGovernorate != null)
                    {
                        birthLocality = await _lookup_localityRepository.GetAll().Where(e => e.NameAr.Contains(propertyList["BirthCityName"].ToString())).FirstOrDefaultAsync();

                        if (birthLocality == null)
                        {
                            birthLocality = await _lookup_localityRepository.GetAll().Where(e => e.NameEn.Contains(propertyList["CityName_EN"].ToString())).FirstOrDefaultAsync();
                        }
                    }
                }
                else
                {
                    birthLocality = await _lookup_localityRepository.GetAll().Where(e => e.NameEn.Contains(propertyList["CityName_EN"].ToString())).FirstOrDefaultAsync();

                    if (birthLocality != null)
                    {
                        birthGovernorate = await _lookup_governorateRepository.GetAll().Where(e => e.Id == birthLocality.GovernorateId).FirstOrDefaultAsync();

                        if (birthGovernorate != null)
                            birthCountry = await _lookup_countryRepository.GetAsync(birthGovernorate.CountryId);
                    }
                }



                var passport = await GetPassportInfo(input);

                person.IdentificationTypeId = input.IdentificationDocumentNoTypeId;
                if (obj == null)
                {
                    person = new Applicant();
                    person.DocumentNo = passport.PassportNo;
                    person.FirstName = propertyList["FirstName"].ToString();
                    person.FatherName = propertyList["FatherName"].ToString();
                    person.GrandFatherName = propertyList["GrandFatherName"].ToString();
                    person.FamilyName = propertyList["FamilyName"].ToString();
                    person.FirstNameEn = propertyList["FirstName_EN"]?.ToString();
                    person.FatherNameEn = propertyList["FatherName_EN"]?.ToString();
                    person.GrandFatherNameEn = propertyList["GrandFatherName_EN"]?.ToString();
                    person.FamilyNameEn = propertyList["FamilyName_EN"]?.ToString();
                    person.Gender = (Gender)passport.SexId;
                    person.BirthDate = passport.BirthDate.Value;
                }

                if (person == null)
                {
                    return null;

                }
            }
            else
            {
                person = await _applicantRepository.FirstOrDefaultAsync(p => (p.DocumentNo == input.IdentificationDocumentNoId && p.IdentificationTypeId == input.IdentificationDocumentNoTypeId));
                if (person == null)
                {
                    return null;

                }
            }

            var output = new GetApplicantForEditOutput { Applicant = ObjectMapper.Map<CreateOrEditApplicantDto>(person) };


            if (birthCountry != null)
            {
                output.Applicant.CountryId = birthCountry.Id;
                output.CountryName = CultureInfo.CurrentUICulture.Name == "ar" ? birthCountry.NameAr : birthCountry.NameEn;
            }

            if (birthGovernorate != null)
            {
                output.Applicant.GovernorateId = birthGovernorate.Id;
                output.GovernorateName = CultureInfo.CurrentUICulture.Name == "ar" ? birthGovernorate.NameAr : birthGovernorate.NameEn;
            }

            if (birthLocality != null)
            {
                output.Applicant.LocalityId = birthLocality.Id;
                output.LocalityName = CultureInfo.CurrentUICulture.Name == "ar" ? birthLocality.NameAr : birthLocality.NameEn;
            }

            var citizenPhoto = await GetCitizenPhoto(input);

            output.CitizenPicture = citizenPhoto.CitizenPicture;

            return output;

        }

        public async Task<PassportInfoDto> GetPassportInfo(FetchPersonDto input)
        {
            var passport = new PassportInfoDto();


            var obj = await _xRoadServicesAppService.GetDynamic(XRoadServiceConsts.MOIPassportInfo, "<IDNo>" + input.IdentificationDocumentNoId + "</IDNo>");

            ExpandoObject propertyNames = (ExpandoObject)obj.Where(v => v.Key == "response").Select(x => x.Value).FirstOrDefault();
            ExpandoObject view = (ExpandoObject)propertyNames.Where(v => v.Key == "passport").Select(x => x.Value).FirstOrDefault();
            var propertyList = (IDictionary<String, Object>)view;

            if (propertyList["PassportNo"] != null)
            {
                passport.PassportNo = propertyList["PassportNo"].ToString();
                passport.IssueDate = DateTime.Parse(propertyList["IssueDate"].ToString(), null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                passport.ExpireDate = DateTime.Parse(propertyList["ExpireDate"].ToString(), null, System.Globalization.DateTimeStyles.AdjustToUniversal);


                passport.FirstName = propertyList["FirstName"].ToString();
                passport.FatherName = propertyList["FatherName"].ToString();
                passport.GrandFatherName = propertyList["GrandFatherName"].ToString();
                passport.FamilyName = propertyList["FamilyName"].ToString();
                passport.MotherName = propertyList["MotherName"].ToString();
                passport.FirstNameEN = propertyList["FirstNameEN"].ToString();
                passport.GrandFatherNameEN = propertyList["GrandFatherNameEN"].ToString();
                passport.FamilyNameEN = propertyList["FamilyNameEN"].ToString();
                passport.MotherNameEN = propertyList["MotherNameEN"].ToString();

                passport.BirthDate = DateTime.Parse(propertyList["BirthDate"].ToString(), null, System.Globalization.DateTimeStyles.AdjustToUniversal);

                passport.SexId = int.Parse(propertyList["Sex"].ToString());
            }

            return passport;
        }
        private async Task<CitizenPhotoDto> GetCitizenPhoto(FetchPersonDto input)
        {
            var pictureInfo = new CitizenPhotoDto();
            var obj = await _xRoadServicesAppService.GetDynamic(XRoadServiceConsts.MOICitizenPhoto, "<IDNo>" + input.IdentificationDocumentNoId + "</IDNo>");
            ExpandoObject propertyNames = (ExpandoObject)obj.Where(v => v.Key == "response").Select(x => x.Value).FirstOrDefault();
            ExpandoObject view = (ExpandoObject)propertyNames.Where(v => v.Key == "service1Object").Select(x => x.Value).FirstOrDefault();
            var propertyList = (IDictionary<String, Object>)view;
            if (propertyList != null)
            {
                pictureInfo.CitizenPicture = propertyList.ContainsKey("ObjectContent") ? propertyList["ObjectContent"]?.ToString() : "";

            }
            return pictureInfo;
        }
    }
}
