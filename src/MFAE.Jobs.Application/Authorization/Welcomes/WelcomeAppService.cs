using Abp.Zero.Configuration;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Authorization.Users.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using MFAE.Jobs.Authorization.Roles;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using System.Collections.Generic;
using System.Globalization;
using Stripe;
using MFAE.Jobs.Location;

namespace MFAE.Jobs.Authorization.Welcomes
{
    public class WelcomeAppService : JobsAppServiceBase, IWelcomeAppService
    {
        private readonly IApplicantsAppService _applicantsAppService;  
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<JobAdvertisement> _jobAdvertisementRepository;
        private readonly IRepository<IdentificationType> _lookup_identificationTypeRepository;
        private readonly IRepository<Applicant, long> _applicantRepository;
        private readonly IRepository<MaritalStatus, int> _lookup_maritalStatusRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<ApplicantStatus, long> _lookup_applicantStatusRepository;
        private readonly IRepository<Country, int> _lookup_countryRepository;
        private readonly IRepository<Governorate, int> _lookup_governorateRepository;
        private readonly IRepository<Locality, int> _lookup_localityRepository;
        private readonly IRepository<ApplicantStudy, long> _applicantStudyRepository;
        private readonly IRepository<ApplicantTraining, long> _applicantTrainingRepository;
        private readonly IRepository<ApplicantLanguage, long> _applicantLanguageRepository;

        public WelcomeAppService(
            IApplicantsAppService applicantsAppService,
            IRepository<ApplicantLanguage, long> applicantLanguageRepository,
            IRepository<ApplicantTraining, long> applicantTrainingRepository,
            IRepository<ApplicantStudy, long> applicantStudyRepository,
            IRepository<Country, int> lookup_countryRepository,
            IRepository<Governorate, int> lookup_governorateRepository,
            IRepository<Locality, int> lookup_localityRepository,
            IRepository<ApplicantStatus, long> lookup_applicantStatusRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<MaritalStatus, int> lookup_maritalStatusRepository,
            IRepository<Applicant, long> applicantRepository,
            IRepository<IdentificationType> lookup_identificationTypeRepository,
            IRepository<JobAdvertisement> jobAdvertisementRepository,
            IRoleManagementConfig roleManagementConfig, 
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<RolePermissionSetting, long> rolePermissionRepository
             
        )
        {
            _applicantsAppService = applicantsAppService;
            _applicantLanguageRepository = applicantLanguageRepository;
            _applicantTrainingRepository = applicantTrainingRepository;
            _applicantStudyRepository = applicantStudyRepository;
            _lookup_localityRepository = lookup_localityRepository;
            _lookup_governorateRepository = lookup_governorateRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_applicantStatusRepository = lookup_applicantStatusRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_maritalStatusRepository = lookup_maritalStatusRepository;
            _applicantRepository = applicantRepository;
            _jobAdvertisementRepository = jobAdvertisementRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleManagementConfig = roleManagementConfig;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userPermissionRepository = userPermissionRepository;
            _lookup_identificationTypeRepository = lookup_identificationTypeRepository;
        }

        public async Task<WelcomeUserDto> GetWelcomeUserForView()
        {
            var query = GetWelcomeUsersFilteredQuery(new GetUsersInput() { UserId = AbpSession.UserId });

            var users = await query.FirstOrDefaultAsync();

            var jobAdvertisement = await GetLastJobAdvertisement();

            var applicantForViewDto = await GetApplicantForLastJobAdvertisementByUserId(users.Id);

            var isApplicant = (applicantForViewDto != null && applicantForViewDto.Applicant != null ? true : false);

            if (isApplicant == false && users.IdentificationTypeId == PersonNationalityConsts.palestinianIdentityTypeID) 
            {
              var fetchApplicant = await _applicantsAppService.FetchPerson(new FetchPersonDto() { IdentificationDocumentNoId = users.DocumentNo, IdentificationDocumentNoTypeId = (int)users.IdentificationTypeId });
             
              if(fetchApplicant != null)
              {
                  applicantForViewDto = new GetApplicantForViewDto { Applicant = ObjectMapper.Map<ApplicantDto>(fetchApplicant.Applicant)};
                  applicantForViewDto.LocalityName = fetchApplicant.LocalityName;
                  applicantForViewDto.GovernorateName = fetchApplicant.GovernorateName;
                  applicantForViewDto.CountryName = fetchApplicant.CountryName;
                  if (applicantForViewDto.Applicant != null && applicantForViewDto.Applicant.Id > 0)
                  {
                     var applicantStudiesCount = await _applicantStudyRepository.GetAll().Where(x => x.ApplicantId == applicantForViewDto.Applicant.Id).CountAsync();
                     applicantForViewDto.applicantStudiesCount = applicantStudiesCount;

                     var applicantTrainingCount = await _applicantTrainingRepository.GetAll().Where(x => x.ApplicantId == applicantForViewDto.Applicant.Id).CountAsync();
                      applicantForViewDto.applicantTrainingCount = applicantTrainingCount;

                     var applicantLanguageCount = await _applicantLanguageRepository.GetAll().Where(x => x.ApplicantId == applicantForViewDto.Applicant.Id).CountAsync();
                     applicantForViewDto.applicantLanguageCount = applicantLanguageCount;

                     if (applicantForViewDto.Applicant.CurrentStatusId != null)
                     {
                         var _lookupApplicantStatus = await _lookup_applicantStatusRepository.FirstOrDefaultAsync((long)applicantForViewDto.Applicant.CurrentStatusId);
                         applicantForViewDto.ApplicantStatusDescription = _lookupApplicantStatus?.Description?.ToString();
                         applicantForViewDto.CurrentStatus = _lookupApplicantStatus.Status;
                     }
                  }
              }
            }

            return new WelcomeUserDto() { User = ObjectMapper.Map<UserListDto>(users) , JobAdvertisement = jobAdvertisement , ApplicantForViewDto = applicantForViewDto , IsApplicant  = isApplicant };
        }


        public async Task<GetApplicantForViewDto> GetApplicantForLastJobAdvertisementByUserId(long userId)
        {
            var applicant = await _applicantRepository.GetAll().Where(x => x.UserId == userId).OrderByDescending(x => x.CreationTime).FirstOrDefaultAsync();

            var output = new GetApplicantForViewDto { Applicant = ObjectMapper.Map<ApplicantDto>(applicant) };

            if(applicant != null)
            {
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

                if (output.Applicant != null)
                {
                    var applicantStudiesCount = await _applicantStudyRepository.GetAll().Where(x => x.ApplicantId == output.Applicant.Id).CountAsync();
                    output.applicantStudiesCount = applicantStudiesCount;

                    var applicantTrainingCount = await _applicantTrainingRepository.GetAll().Where(x => x.ApplicantId == output.Applicant.Id).CountAsync();
                    output.applicantTrainingCount = applicantTrainingCount;

                    var applicantLanguageCount = await _applicantLanguageRepository.GetAll().Where(x => x.ApplicantId == output.Applicant.Id).CountAsync();
                    output.applicantLanguageCount = applicantLanguageCount;

                }
            }

            return output;
        }

        public async Task<JobAdvertisementDto> GetLastJobAdvertisement()
        {
            return await _jobAdvertisementRepository.GetAll().OrderByDescending(x => x.CreationTime)
                .Where(x => x.IsActive == true)
                .Select(jobAd => new JobAdvertisementDto
                {
                    Description = jobAd.Description,
                    Id = jobAd.Id,
                }).FirstOrDefaultAsync();
        }



        private IQueryable<User> GetWelcomeUsersFilteredQuery(IGetUsersInput input)
        {
            var query = UserManager.Users
                .WhereIf(input.UserId.HasValue, u => u.Id == input.UserId)
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(input.OnlyLockedUsers,
                    u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            if (input.Permissions != null && input.Permissions.Any(p => !p.IsNullOrWhiteSpace()))
            {
                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                    r => r.GrantAllPermissionsByDefault &&
                         r.Side == AbpSession.MultiTenancySide
                ).Select(r => r.RoleName).ToList();

                input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

                var userIds = from user in query
                              join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                              from ur in urJoined.DefaultIfEmpty()
                              join urr in _roleRepository.GetAll() on ur.RoleId equals urr.Id into urrJoined
                              from urr in urrJoined.DefaultIfEmpty()
                              join up in _userPermissionRepository.GetAll()
                                  .Where(userPermission => input.Permissions.Contains(userPermission.Name)) on user.Id equals up.UserId into upJoined
                              from up in upJoined.DefaultIfEmpty()
                              join rp in _rolePermissionRepository.GetAll()
                                  .Where(rolePermission => input.Permissions.Contains(rolePermission.Name)) on
                                  new { RoleId = ur == null ? 0 : ur.RoleId } equals new { rp.RoleId } into rpJoined
                              from rp in rpJoined.DefaultIfEmpty()
                              where (up != null && up.IsGranted) ||
                                    (up == null && rp != null && rp.IsGranted) ||
                                    (up == null && rp == null && staticRoleNames.Contains(urr.Name))
                              group user by user.Id
                    into userGrouped
                              select userGrouped.Key;

                query = UserManager.Users.Where(e => userIds.Contains(e.Id));
            }

            return query;
        }

    }
}
