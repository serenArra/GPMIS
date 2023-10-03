using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Location;

using MFAE.Jobs.ApplicationForm.Enums;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MFAE.Jobs.ApplicationForm.Exporting;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.XRoad;
using System.Dynamic;
using System.Globalization;
using Abp.Authorization.Users;
using Abp.Runtime.Session;
using MFAE.Jobs.Authorization.Users.Dto;
using MFAE.Jobs.Url;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Abp.Extensions;
using Microsoft.AspNetCore.Identity;
using MFAE.Jobs.Authorization.Roles;
using Abp.Notifications;
using MFAE.Jobs.Notifications;
using Abp.Domain.Uow;
using System.Transactions;


namespace MFAE.Jobs.ApplicationForm
{
    [AbpAuthorize(AppPermissions.Pages_Applicants)]
    public class ApplicantsAppService : JobsAppServiceBase, IApplicantsAppService
    {
        public IAppUrlService AppUrlService { get; set; }

        private readonly IRepository<Applicant, long> _applicantRepository;
        private readonly IApplicantsExcelExporter _applicantsExcelExporter;
        private readonly IRepository<IdentificationType, int> _lookup_identificationTypeRepository;
        private readonly IRepository<MaritalStatus, int> _lookup_maritalStatusRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<ApplicantStatus, long> _lookup_applicantStatusRepository;
        private readonly IRepository<Country, int> _lookup_countryRepository;
        private readonly IRepository<Governorate, int> _lookup_governorateRepository;
        private readonly IRepository<Locality, int> _lookup_localityRepository;
        private readonly IXRoadServicesAppService _xRoadServicesAppService;
        private readonly IUserEmailer _userEmailer;
        private readonly IUserPolicy _userPolicy;
        private readonly UserManager _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RoleManager _roleManager;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
      


        public ApplicantsAppService(IUnitOfWorkManager unitOfWorkManager, IAppNotifier appNotifier, INotificationSubscriptionManager notificationSubscriptionManager, RoleManager roleManager, IEnumerable<IPasswordValidator<User>> passwordValidators, IUserPolicy userPolicy, UserManager userManager, IPasswordHasher<User> passwordHasher, IUserEmailer userEmailer, IXRoadServicesAppService xRoadServicesAppService, IRepository<Applicant, long> applicantRepository, IApplicantsExcelExporter applicantsExcelExporter, IRepository<IdentificationType, int> lookup_identificationTypeRepository, IRepository<MaritalStatus, int> lookup_maritalStatusRepository, IRepository<User, long> lookup_userRepository, IRepository<ApplicantStatus, long> lookup_applicantStatusRepository, IRepository<Country, int> lookup_countryRepository, IRepository<Governorate, int> lookup_governorateRepository, IRepository<Locality, int> lookup_localityRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _appNotifier = appNotifier;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _roleManager = roleManager;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _userPolicy = userPolicy;
            _userEmailer = userEmailer;
            _applicantRepository = applicantRepository;
            _applicantsExcelExporter = applicantsExcelExporter;
            _lookup_identificationTypeRepository = lookup_identificationTypeRepository;
            _lookup_maritalStatusRepository = lookup_maritalStatusRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_applicantStatusRepository = lookup_applicantStatusRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_governorateRepository = lookup_governorateRepository;
            _lookup_localityRepository = lookup_localityRepository;
            _xRoadServicesAppService = xRoadServicesAppService;

        }

        public async Task<PagedResultDto<GetApplicantForViewDto>> GetAll(GetAllApplicantsInput input)
        {
            var genderFilter = input.GenderFilter.HasValue
                        ? (Gender)input.GenderFilter
                        : default;

            var filteredApplicants = _applicantRepository.GetAll()
                        .Include(e => e.IdentificationTypeFk)
                        .Include(e => e.MaritalStatusFk)
                        .Include(e => e.LockedByFk)
                        .Include(e => e.CurrentStatusFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.GovernorateFk)
                        .Include(e => e.LocalityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentNo.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.FatherName.Contains(input.Filter) || e.GrandFatherName.Contains(input.Filter) || e.FamilyName.Contains(input.Filter) || e.FirstNameEn.Contains(input.Filter) || e.FatherNameEn.Contains(input.Filter) || e.GrandFatherNameEn.Contains(input.Filter) || e.FamilyNameEn.Contains(input.Filter) || e.BirthPlace.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNoFilter), e => e.DocumentNo.Contains(input.DocumentNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FatherNameFilter), e => e.FatherName.Contains(input.FatherNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GrandFatherNameFilter), e => e.GrandFatherName.Contains(input.GrandFatherNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FamilyNameFilter), e => e.FamilyName.Contains(input.FamilyNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameEnFilter), e => e.FirstNameEn.Contains(input.FirstNameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FatherNameEnFilter), e => e.FatherNameEn.Contains(input.FatherNameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GrandFatherNameEnFilter), e => e.GrandFatherNameEn.Contains(input.GrandFatherNameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FamilyNameEnFilter), e => e.FamilyNameEn.Contains(input.FamilyNameEnFilter))
                        .WhereIf(input.MinBirthDateFilter != null, e => e.BirthDate >= input.MinBirthDateFilter)
                        .WhereIf(input.MaxBirthDateFilter != null, e => e.BirthDate <= input.MaxBirthDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BirthPlaceFilter), e => e.BirthPlace.Contains(input.BirthPlaceFilter))
                        .WhereIf(input.MinNumberOfChildrenFilter != null, e => e.NumberOfChildren >= input.MinNumberOfChildrenFilter)
                        .WhereIf(input.MaxNumberOfChildrenFilter != null, e => e.NumberOfChildren <= input.MaxNumberOfChildrenFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(input.IsLockedFilter.HasValue && input.IsLockedFilter > -1, e => (input.IsLockedFilter == 1 && e.IsLocked) || (input.IsLockedFilter == 0 && !e.IsLocked))
                        .WhereIf(input.GenderFilter.HasValue && input.GenderFilter > -1, e => e.Gender == genderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IdentificationTypeNameFilter), e => e.IdentificationTypeFk != null && e.IdentificationTypeFk.Name == input.IdentificationTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MaritalStatusNameFilter), e => e.MaritalStatusFk != null && e.MaritalStatusFk.Name == input.MaritalStatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.LockedByFk != null && e.LockedByFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantStatusDescriptionFilter), e => e.CurrentStatusFk != null && e.CurrentStatusFk.Description == input.ApplicantStatusDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovernorateNameFilter), e => e.GovernorateFk != null && e.GovernorateFk.Name == input.GovernorateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocalityNameFilter), e => e.LocalityFk != null && e.LocalityFk.Name == input.LocalityNameFilter);

            var pagedAndFilteredApplicants = filteredApplicants
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var applicants = from o in pagedAndFilteredApplicants
                             join o1 in _lookup_identificationTypeRepository.GetAll() on o.IdentificationTypeId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_maritalStatusRepository.GetAll() on o.MaritalStatusId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             join o3 in _lookup_userRepository.GetAll() on o.LockedBy equals o3.Id into j3
                             from s3 in j3.DefaultIfEmpty()

                             join o4 in _lookup_applicantStatusRepository.GetAll() on o.CurrentStatusId equals o4.Id into j4
                             from s4 in j4.DefaultIfEmpty()

                             join o5 in _lookup_countryRepository.GetAll() on o.CountryId equals o5.Id into j5
                             from s5 in j5.DefaultIfEmpty()

                             join o6 in _lookup_governorateRepository.GetAll() on o.GovernorateId equals o6.Id into j6
                             from s6 in j6.DefaultIfEmpty()

                             join o7 in _lookup_localityRepository.GetAll() on o.LocalityId equals o7.Id into j7
                             from s7 in j7.DefaultIfEmpty()

                             select new
                             {

                                 o.DocumentNo,
                                 o.FirstName,
                                 o.FatherName,
                                 o.GrandFatherName,
                                 o.FamilyName,
                                 o.FirstNameEn,
                                 o.FatherNameEn,
                                 o.GrandFatherNameEn,
                                 o.FamilyNameEn,
                                 o.BirthDate,
                                 o.BirthPlace,
                                 o.NumberOfChildren,
                                 o.Address,
                                 o.Mobile,
                                 o.Email,
                                 o.IsLocked,
                                 o.Gender,
                                 Id = o.Id,
                                 IdentificationTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 MaritalStatusName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                 UserName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                 ApplicantStatusDescription = s4 == null || s4.Description == null ? "" : s4.Description.ToString(),
                                 CountryName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                                 GovernorateName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                                 LocalityName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                             };

            var totalCount = await filteredApplicants.CountAsync();

            var dbList = await applicants.ToListAsync();
            var results = new List<GetApplicantForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetApplicantForViewDto()
                {
                    Applicant = new ApplicantDto
                    {

                        DocumentNo = o.DocumentNo,
                        FirstName = o.FirstName,
                        FatherName = o.FatherName,
                        GrandFatherName = o.GrandFatherName,
                        FamilyName = o.FamilyName,
                        FirstNameEn = o.FirstNameEn,
                        FatherNameEn = o.FatherNameEn,
                        GrandFatherNameEn = o.GrandFatherNameEn,
                        FamilyNameEn = o.FamilyNameEn,
                        BirthDate = o.BirthDate,
                        BirthPlace = o.BirthPlace,
                        NumberOfChildren = o.NumberOfChildren,
                        Address = o.Address,
                        Mobile = o.Mobile,
                        Email = o.Email,
                        IsLocked = o.IsLocked,
                        Gender = o.Gender,
                        Id = o.Id,
                    },
                    IdentificationTypeName = o.IdentificationTypeName,
                    MaritalStatusName = o.MaritalStatusName,
                    UserName = o.UserName,
                    ApplicantStatusDescription = o.ApplicantStatusDescription,
                    CountryName = o.CountryName,
                    GovernorateName = o.GovernorateName,
                    LocalityName = o.LocalityName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetApplicantForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetApplicantForViewDto> GetApplicantForView(long id)
        {
            var applicant = await _applicantRepository.GetAsync(id);

            var output = new GetApplicantForViewDto { Applicant = ObjectMapper.Map<ApplicantDto>(applicant) };

            if (output.Applicant.IdentificationTypeId > 0)
            {
                var _lookupIdentificationType = await _lookup_identificationTypeRepository.FirstOrDefaultAsync((int)output.Applicant.IdentificationTypeId);
                output.IdentificationTypeName = _lookupIdentificationType?.Name?.ToString();
            }

            if (output.Applicant.MaritalStatusId > 0)
            {
                var _lookupMaritalStatus = await _lookup_maritalStatusRepository.FirstOrDefaultAsync((int)output.Applicant.MaritalStatusId);
                output.MaritalStatusName = _lookupMaritalStatus?.Name?.ToString();
            }

            if (output.Applicant.LockedBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Applicant.LockedBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Applicant.CurrentStatusId != null)
            {
                var _lookupApplicantStatus = await _lookup_applicantStatusRepository.FirstOrDefaultAsync((long)output.Applicant.CurrentStatusId);
                output.ApplicantStatusDescription = _lookupApplicantStatus?.Description?.ToString();
            }

            if (output.Applicant.CountryId > 0)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.Applicant.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Applicant.GovernorateId != null)
            {
                var _lookupGovernorate = await _lookup_governorateRepository.FirstOrDefaultAsync((int)output.Applicant.GovernorateId);
                output.GovernorateName = _lookupGovernorate?.Name?.ToString();
            }

            if (output.Applicant.LocalityId != null)
            {
                var _lookupLocality = await _lookup_localityRepository.FirstOrDefaultAsync((int)output.Applicant.LocalityId);
                output.LocalityName = _lookupLocality?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Edit)]
        public async Task<GetApplicantForEditOutput> GetApplicantForEdit(EntityDto<long> input)
        {
            var applicant = await _applicantRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicantForEditOutput { Applicant = ObjectMapper.Map<CreateOrEditApplicantDto>(applicant) };

            if (output.Applicant.IdentificationTypeId > 0)
            {
                var _lookupIdentificationType = await _lookup_identificationTypeRepository.FirstOrDefaultAsync((int)output.Applicant.IdentificationTypeId);
                output.IdentificationTypeName = _lookupIdentificationType?.Name?.ToString();
            }

            if (output.Applicant.MaritalStatusId > 0)
            {
                var _lookupMaritalStatus = await _lookup_maritalStatusRepository.FirstOrDefaultAsync((int)output.Applicant.MaritalStatusId);
                output.MaritalStatusName = _lookupMaritalStatus?.Name?.ToString();
            }

            if (output.Applicant.LockedBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Applicant.LockedBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Applicant.CurrentStatusId != null)
            {
                var _lookupApplicantStatus = await _lookup_applicantStatusRepository.FirstOrDefaultAsync((long)output.Applicant.CurrentStatusId);
                output.ApplicantStatusDescription = _lookupApplicantStatus?.Description?.ToString();
            }

            if (output.Applicant.CountryId > 0)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.Applicant.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Applicant.GovernorateId != null)
            {
                var _lookupGovernorate = await _lookup_governorateRepository.FirstOrDefaultAsync((int)output.Applicant.GovernorateId);
                output.GovernorateName = _lookupGovernorate?.Name?.ToString();
            }

            if (output.Applicant.LocalityId != null)
            {
                var _lookupLocality = await _lookup_localityRepository.FirstOrDefaultAsync((int)output.Applicant.LocalityId);
                output.LocalityName = _lookupLocality?.Name?.ToString();
            }

            return output;
        }

        public async Task<long> CreateOrEdit(CreateOrEditApplicantDto input)
        {
            if (input.Id == null)
            {
              return await Create(input);
            }
            else
            {
                await Update(input);
                return (long)input.Id;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Create)]
        protected virtual async Task<long> Create(CreateOrEditApplicantDto input)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew)) {
                var applicant = ObjectMapper.Map<Applicant>(input);
                applicant.Id = await _applicantRepository.InsertAndGetIdAsync(applicant);

                var applicantStatus = new ApplicantStatus()
                {
                    Status = ApplicantStatusEnum.New,
                    Description = L("NewApplicantAdded"),
                    ApplicantId = applicant.Id,
                    CreatorUserId = AbpSession.UserId,
                };

                applicant.CurrentStatusId = await _lookup_applicantStatusRepository.InsertAndGetIdAsync(applicantStatus);

                await _applicantRepository.UpdateAsync(applicant);
                await CurrentUnitOfWork.SaveChangesAsync();

                /* var UserRecorde = await _lookup_userRepository.GetAll().Where(x => x.Id == input.UserId).FirstOrDefaultAsync();
                 var userRole = new string[] { "User" };

                 CreateOrUpdateUserInput user = new CreateOrUpdateUserInput
                 {
                     User = new UserEditDto
                     {
                         EmailAddress = input.Email,
                         PhoneNumber = input.Mobile,
                         Name = input.FirstNameBylanguage,
                         Surname = input.FamilyNameBylanguage,
                         UserName = input.FirstName,
                       *//*  Password = input.Password,
                         IsActive = input.IsActive,
                         IsLockoutEnabled = input.IsLockoutEnabled,
                         IsTwoFactorEnabled = input.IsTwoFactorEnabled,*//*
                     },
                     SetRandomPassword = input.SetRandomPassword,
                     SendActivationEmail = input.SendActivationEmail,
                     AssignedRoleNames = userRole,
                 };

                 //From Existance User Recorde
                 if (UserRecorde != null)
                 {
                     var _userRole = await UserManager.GetRolesAsync(UserRecorde);
                     user.User.Id = UserRecorde.Id;
                     user.AssignedRoleNames = _userRole.ToArray();
                 }

                 applicant.UserId = await CreateOrUpdateUser(user);*/
                uow.Complete();
                return applicant.Id;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Edit)]
        protected virtual async Task Update(CreateOrEditApplicantDto input)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var applicant = await _applicantRepository.FirstOrDefaultAsync((long)input.Id);
                input.CurrentStatusId = applicant.CurrentStatusId;
                ObjectMapper.Map(input, applicant);

                uow.Complete();
            }
           

        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _applicantRepository.DeleteAsync(input.Id);
        }

        protected async Task<long> CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);

                return input.User.Id.Value;
            }
            else
            {
                return await CreateUserAsync(input);
            }
        }


        [AbpAuthorize(AppPermissions.Pages_Applicants_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await UserManager.FindByIdAsync(input.User.Id.Value.ToString());

            //Update user properties
            ObjectMapper.Map(input.User, user); //Passwords is not mapped (see mapping configuration)

            CheckErrors(await UserManager.UpdateAsync(user));

            if (input.SetRandomPassword)
            {
                var randomPassword = await _userManager.CreateRandomPassword();
                user.Password = _passwordHasher.HashPassword(user, randomPassword);
                input.User.Password = randomPassword;
            }
            else if (!input.User.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
            }

            //Update roles
            CheckErrors(await UserManager.SetRolesAsync(user, input.AssignedRoleNames));

            //update organization units
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(
                    user,
                    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                    input.User.Password
                );
            }
        }


        [AbpAuthorize(AppPermissions.Pages_Applicants_Create)]
        protected virtual async Task<long> CreateUserAsync(CreateOrUpdateUserInput input)
        {
            if (AbpSession.TenantId.HasValue)
            {
                await _userPolicy.CheckMaxUserCountAsync(AbpSession.GetTenantId());
            }

            var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
            user.TenantId = AbpSession.TenantId;

            //Set password
            if (input.SetRandomPassword)
            {
                var randomPassword = await _userManager.CreateRandomPassword();
                user.Password = _passwordHasher.HashPassword(user, randomPassword);
                input.User.Password = randomPassword;
            }
            else if (!input.User.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                foreach (var validator in _passwordValidators)
                {
                    CheckErrors(await validator.ValidateAsync(UserManager, user, input.User.Password));
                }

                user.Password = _passwordHasher.HashPassword(user, input.User.Password);
            }

            user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

            //Assign roles
            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }

            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);

            //Organization Units
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            //Send activation email
            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(
                    user,
                    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                    input.User.Password
                );
            }

            return user.Id;
        }

        public async Task<FileDto> GetApplicantsToExcel(GetAllApplicantsForExcelInput input)
        {
            var genderFilter = input.GenderFilter.HasValue
                        ? (Gender)input.GenderFilter
                        : default;

            var filteredApplicants = _applicantRepository.GetAll()
                        .Include(e => e.IdentificationTypeFk)
                        .Include(e => e.MaritalStatusFk)
                        .Include(e => e.LockedByFk)
                        .Include(e => e.CurrentStatusFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.GovernorateFk)
                        .Include(e => e.LocalityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentNo.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.FatherName.Contains(input.Filter) || e.GrandFatherName.Contains(input.Filter) || e.FamilyName.Contains(input.Filter) || e.FirstNameEn.Contains(input.Filter) || e.FatherNameEn.Contains(input.Filter) || e.GrandFatherNameEn.Contains(input.Filter) || e.FamilyNameEn.Contains(input.Filter) || e.BirthPlace.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNoFilter), e => e.DocumentNo.Contains(input.DocumentNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FatherNameFilter), e => e.FatherName.Contains(input.FatherNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GrandFatherNameFilter), e => e.GrandFatherName.Contains(input.GrandFatherNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FamilyNameFilter), e => e.FamilyName.Contains(input.FamilyNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameEnFilter), e => e.FirstNameEn.Contains(input.FirstNameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FatherNameEnFilter), e => e.FatherNameEn.Contains(input.FatherNameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GrandFatherNameEnFilter), e => e.GrandFatherNameEn.Contains(input.GrandFatherNameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FamilyNameEnFilter), e => e.FamilyNameEn.Contains(input.FamilyNameEnFilter))
                        .WhereIf(input.MinBirthDateFilter != null, e => e.BirthDate >= input.MinBirthDateFilter)
                        .WhereIf(input.MaxBirthDateFilter != null, e => e.BirthDate <= input.MaxBirthDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BirthPlaceFilter), e => e.BirthPlace.Contains(input.BirthPlaceFilter))
                        .WhereIf(input.MinNumberOfChildrenFilter != null, e => e.NumberOfChildren >= input.MinNumberOfChildrenFilter)
                        .WhereIf(input.MaxNumberOfChildrenFilter != null, e => e.NumberOfChildren <= input.MaxNumberOfChildrenFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(input.IsLockedFilter.HasValue && input.IsLockedFilter > -1, e => (input.IsLockedFilter == 1 && e.IsLocked) || (input.IsLockedFilter == 0 && !e.IsLocked))
                        .WhereIf(input.GenderFilter.HasValue && input.GenderFilter > -1, e => e.Gender == genderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IdentificationTypeNameFilter), e => e.IdentificationTypeFk != null && e.IdentificationTypeFk.Name == input.IdentificationTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MaritalStatusNameFilter), e => e.MaritalStatusFk != null && e.MaritalStatusFk.Name == input.MaritalStatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.LockedByFk != null && e.LockedByFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantStatusDescriptionFilter), e => e.CurrentStatusFk != null && e.CurrentStatusFk.Description == input.ApplicantStatusDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovernorateNameFilter), e => e.GovernorateFk != null && e.GovernorateFk.Name == input.GovernorateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocalityNameFilter), e => e.LocalityFk != null && e.LocalityFk.Name == input.LocalityNameFilter);

            var query = (from o in filteredApplicants
                         join o1 in _lookup_identificationTypeRepository.GetAll() on o.IdentificationTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_maritalStatusRepository.GetAll() on o.MaritalStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_userRepository.GetAll() on o.LockedBy equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_applicantStatusRepository.GetAll() on o.CurrentStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_countryRepository.GetAll() on o.CountryId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_governorateRepository.GetAll() on o.GovernorateId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_localityRepository.GetAll() on o.LocalityId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetApplicantForViewDto()
                         {
                             Applicant = new ApplicantDto
                             {
                                 DocumentNo = o.DocumentNo,
                                 FirstName = o.FirstName,
                                 FatherName = o.FatherName,
                                 GrandFatherName = o.GrandFatherName,
                                 FamilyName = o.FamilyName,
                                 FirstNameEn = o.FirstNameEn,
                                 FatherNameEn = o.FatherNameEn,
                                 GrandFatherNameEn = o.GrandFatherNameEn,
                                 FamilyNameEn = o.FamilyNameEn,
                                 BirthDate = o.BirthDate,
                                 BirthPlace = o.BirthPlace,
                                 NumberOfChildren = o.NumberOfChildren,
                                 Address = o.Address,
                                 Mobile = o.Mobile,
                                 Email = o.Email,
                                 IsLocked = o.IsLocked,
                                 Gender = o.Gender,
                                 Id = o.Id
                             },
                             IdentificationTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MaritalStatusName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             UserName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             ApplicantStatusDescription = s4 == null || s4.Description == null ? "" : s4.Description.ToString(),
                             CountryName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             GovernorateName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                             LocalityName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                         });

            var applicantListDtos = await query.ToListAsync();

            return _applicantsExcelExporter.ExportToFile(applicantListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantIdentificationTypeLookupTableDto>> GetAllIdentificationTypeForTableDropdown()
        {
            return await _lookup_identificationTypeRepository.GetAll().OrderBy(e => CultureInfo.CurrentUICulture.Name == "ar" ? e.NameAr : e.NameEn)
               .Select(identificationType => new ApplicantIdentificationTypeLookupTableDto
               {
                   Id = identificationType.Id,
                   NameAr = identificationType == null || identificationType.NameAr == null ? "" : identificationType.NameAr.ToString(),
                   NameEn = identificationType == null || identificationType.NameEn == null ? "" : identificationType.NameEn.ToString(),
                   IsDefault = identificationType.IsDefault
               }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantMaritalStatusLookupTableDto>> GetAllMaritalStatusForTableDropdown()
        {
            return await _lookup_maritalStatusRepository.GetAll()
                .Select(maritalStatus => new ApplicantMaritalStatusLookupTableDto
                {
                    Id = maritalStatus.Id,
                    DisplayName = maritalStatus == null || maritalStatus.Name == null ? "" : maritalStatus.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new ApplicantUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantApplicantStatusLookupTableDto>> GetAllApplicantStatusForTableDropdown()
        {
            return await _lookup_applicantStatusRepository.GetAll()
                .Select(applicantStatus => new ApplicantApplicantStatusLookupTableDto
                {
                    Id = applicantStatus.Id,
                    DisplayName = applicantStatus == null || applicantStatus.Description == null ? "" : applicantStatus.Description.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new ApplicantCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantGovernorateLookupTableDto>> GetAllGovernorateForTableDropdown()
        {
            return await _lookup_governorateRepository.GetAll()
                .Select(governorate => new ApplicantGovernorateLookupTableDto
                {
                    Id = governorate.Id,
                    DisplayName = governorate == null || governorate.Name == null ? "" : governorate.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantLocalityLookupTableDto>> GetAllLocalityForTableDropdown()
        {
            return await _lookup_localityRepository.GetAll()
                .Select(locality => new ApplicantLocalityLookupTableDto
                {
                    Id = locality.Id,
                    DisplayName = locality == null || locality.Name == null ? "" : locality.Name.ToString()
                }).ToListAsync();
        }
     
        public async Task<GetApplicantForEditOutput> FetchPerson(FetchPersonDto input)
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
                            birthLocality = await _lookup_localityRepository.GetAll().Where(e => e.NameEn.Contains(propertyList["CityName_EN"].ToString()) || e.NameAr.Contains(propertyList["CityName_AR"].ToString())).FirstOrDefaultAsync();
                        }
                    }                    
                }
                else
                {
                    birthLocality = await _lookup_localityRepository.GetAll().Where(e => e.NameEn.Contains(propertyList["CityName_EN"].ToString()) || e.NameAr.Contains(propertyList["CityName_AR"].ToString())).FirstOrDefaultAsync();

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