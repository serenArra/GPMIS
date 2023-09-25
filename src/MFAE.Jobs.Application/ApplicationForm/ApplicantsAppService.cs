using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.Authorization.Users;

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
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.ApplicationForm
{
    [AbpAuthorize(AppPermissions.Pages_Applicants)]
    public class ApplicantsAppService : JobsAppServiceBase, IApplicantsAppService
    {
        private readonly IRepository<Applicant, long> _applicantRepository;
        private readonly IApplicantsExcelExporter _applicantsExcelExporter;
        private readonly IRepository<IdentificationType, int> _lookup_identificationTypeRepository;
        private readonly IRepository<MaritalStatus, int> _lookup_maritalStatusRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<ApplicantStatus, long> _lookup_applicantStatusRepository;

        public ApplicantsAppService(IRepository<Applicant, long> applicantRepository, IApplicantsExcelExporter applicantsExcelExporter, IRepository<IdentificationType, int> lookup_identificationTypeRepository, IRepository<MaritalStatus, int> lookup_maritalStatusRepository, IRepository<User, long> lookup_userRepository, IRepository<ApplicantStatus, long> lookup_applicantStatusRepository)
        {
            _applicantRepository = applicantRepository;
            _applicantsExcelExporter = applicantsExcelExporter;
            _lookup_identificationTypeRepository = lookup_identificationTypeRepository;
            _lookup_maritalStatusRepository = lookup_maritalStatusRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_applicantStatusRepository = lookup_applicantStatusRepository;

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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantStatusDescriptionFilter), e => e.CurrentStatusFk != null && e.CurrentStatusFk.Description == input.ApplicantStatusDescriptionFilter);

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
                                 ApplicantStatusDescription = s4 == null || s4.Description == null ? "" : s4.Description.ToString()
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
                    ApplicantStatusDescription = o.ApplicantStatusDescription
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

            if (output.Applicant.IdentificationTypeId != null)
            {
                var _lookupIdentificationType = await _lookup_identificationTypeRepository.FirstOrDefaultAsync((int)output.Applicant.IdentificationTypeId);
                output.IdentificationTypeName = _lookupIdentificationType?.Name?.ToString();
            }

            if (output.Applicant.MaritalStatusId != null)
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

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Edit)]
        public async Task<GetApplicantForEditOutput> GetApplicantForEdit(EntityDto<long> input)
        {
            var applicant = await _applicantRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicantForEditOutput { Applicant = ObjectMapper.Map<CreateOrEditApplicantDto>(applicant) };

            if (output.Applicant.IdentificationTypeId != null)
            {
                var _lookupIdentificationType = await _lookup_identificationTypeRepository.FirstOrDefaultAsync((int)output.Applicant.IdentificationTypeId);
                output.IdentificationTypeName = _lookupIdentificationType?.Name?.ToString();
            }

            if (output.Applicant.MaritalStatusId != null)
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

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApplicantDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Create)]
        protected virtual async Task Create(CreateOrEditApplicantDto input)
        {
            var applicant = ObjectMapper.Map<Applicant>(input);

            await _applicantRepository.InsertAsync(applicant);

        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Edit)]
        protected virtual async Task Update(CreateOrEditApplicantDto input)
        {
            var applicant = await _applicantRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, applicant);

        }

        [AbpAuthorize(AppPermissions.Pages_Applicants_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _applicantRepository.DeleteAsync(input.Id);
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantStatusDescriptionFilter), e => e.CurrentStatusFk != null && e.CurrentStatusFk.Description == input.ApplicantStatusDescriptionFilter);

            var query = (from o in filteredApplicants
                         join o1 in _lookup_identificationTypeRepository.GetAll() on o.IdentificationTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_maritalStatusRepository.GetAll() on o.MaritalStatusId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_userRepository.GetAll() on o.LockedBy equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_applicantStatusRepository.GetAll() on o.CurrentStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

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
                             ApplicantStatusDescription = s4 == null || s4.Description == null ? "" : s4.Description.ToString()
                         });

            var applicantListDtos = await query.ToListAsync();

            return _applicantsExcelExporter.ExportToFile(applicantListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Applicants)]
        public async Task<List<ApplicantIdentificationTypeLookupTableDto>> GetAllIdentificationTypeForTableDropdown()
        {
            return await _lookup_identificationTypeRepository.GetAll()
                .Select(identificationType => new ApplicantIdentificationTypeLookupTableDto
                {
                    Id = identificationType.Id,
                    DisplayName = identificationType == null || identificationType.Name == null ? "" : identificationType.Name.ToString()
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

    }
}