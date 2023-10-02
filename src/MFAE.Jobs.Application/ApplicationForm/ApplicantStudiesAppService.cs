using MFAE.Jobs.ApplicationForm;

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
    [AbpAuthorize(AppPermissions.Pages_ApplicantStudies)]
    public class ApplicantStudiesAppService : JobsAppServiceBase, IApplicantStudiesAppService
    {
        private readonly IRepository<ApplicantStudy, long> _applicantStudyRepository;
        private readonly IApplicantStudiesExcelExporter _applicantStudiesExcelExporter;
        private readonly IRepository<GraduationRate, int> _lookup_graduationRateRepository;
        private readonly IRepository<AcademicDegree, int> _lookup_academicDegreeRepository;
        private readonly IRepository<Specialties, int> _lookup_specialtiesRepository;
        private readonly IRepository<Applicant, long> _lookup_applicantRepository;

        public ApplicantStudiesAppService(IRepository<ApplicantStudy, long> applicantStudyRepository, IApplicantStudiesExcelExporter applicantStudiesExcelExporter, IRepository<GraduationRate, int> lookup_graduationRateRepository, IRepository<AcademicDegree, int> lookup_academicDegreeRepository, IRepository<Specialties, int> lookup_specialtiesRepository, IRepository<Applicant, long> lookup_applicantRepository)
        {
            _applicantStudyRepository = applicantStudyRepository;
            _applicantStudiesExcelExporter = applicantStudiesExcelExporter;
            _lookup_graduationRateRepository = lookup_graduationRateRepository;
            _lookup_academicDegreeRepository = lookup_academicDegreeRepository;
            _lookup_specialtiesRepository = lookup_specialtiesRepository;
            _lookup_applicantRepository = lookup_applicantRepository;

        }

        public async Task<PagedResultDto<GetApplicantStudyForViewDto>> GetAll(GetAllApplicantStudiesInput input)
        {

            var filteredApplicantStudies = _applicantStudyRepository.GetAll()
                        .Include(e => e.GraduationRateFk)
                        .Include(e => e.AcademicDegreeFk)
                        .Include(e => e.SpecialtiesFk)
                        .Include(e => e.ApplicantFk)
                        .Where(e => e.ApplicantId == input.ApplicantIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OtherSpecialty.Contains(input.Filter) || e.SecondSpecialty.Contains(input.Filter) || e.University.Contains(input.Filter) || e.GraduationCountry.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherSpecialtyFilter), e => e.OtherSpecialty.Contains(input.OtherSpecialtyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SecondSpecialtyFilter), e => e.SecondSpecialty.Contains(input.SecondSpecialtyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversityFilter), e => e.University.Contains(input.UniversityFilter))
                        .WhereIf(input.MinGraduationYearFilter != null, e => e.GraduationYear >= input.MinGraduationYearFilter)
                        .WhereIf(input.MaxGraduationYearFilter != null, e => e.GraduationYear <= input.MaxGraduationYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GraduationCountryFilter), e => e.GraduationCountry.Contains(input.GraduationCountryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GraduationRateNameFilter), e => e.GraduationRateFk != null && e.GraduationRateFk.Name == input.GraduationRateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AcademicDegreeNameFilter), e => e.AcademicDegreeFk != null && e.AcademicDegreeFk.Name == input.AcademicDegreeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SpecialtiesNameFilter), e => e.SpecialtiesFk != null && e.SpecialtiesFk.Name == input.SpecialtiesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFirstNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FirstName == input.ApplicantFirstNameFilter);

            var pagedAndFilteredApplicantStudies = filteredApplicantStudies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var applicantStudies = from o in pagedAndFilteredApplicantStudies
                                   join o1 in _lookup_graduationRateRepository.GetAll() on o.GraduationRateId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_academicDegreeRepository.GetAll() on o.AcademicDegreeId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   join o3 in _lookup_specialtiesRepository.GetAll() on o.SpecialtiesId equals o3.Id into j3
                                   from s3 in j3.DefaultIfEmpty()

                                   join o4 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o4.Id into j4
                                   from s4 in j4.DefaultIfEmpty()

                                   select new
                                   {

                                       o.OtherSpecialty,
                                       o.SecondSpecialty,
                                       o.University,
                                       o.GraduationYear,
                                       o.GraduationCountry,
                                       Id = o.Id,
                                       GraduationRateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       AcademicDegreeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                       SpecialtiesName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                       ApplicantFirstName = s4 == null || s4.FirstName == null ? "" : s4.FirstName.ToString()
                                   };

            var totalCount = await filteredApplicantStudies.CountAsync();

            var dbList = await applicantStudies.ToListAsync();
            var results = new List<GetApplicantStudyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetApplicantStudyForViewDto()
                {
                    ApplicantStudy = new ApplicantStudyDto
                    {

                        OtherSpecialty = o.OtherSpecialty,
                        SecondSpecialty = o.SecondSpecialty,
                        University = o.University,
                        GraduationYear = o.GraduationYear,
                        GraduationCountry = o.GraduationCountry,
                        Id = o.Id,
                    },
                    GraduationRateName = o.GraduationRateName,
                    AcademicDegreeName = o.AcademicDegreeName,
                    SpecialtiesName = o.SpecialtiesName,
                    ApplicantFirstName = o.ApplicantFirstName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetApplicantStudyForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetApplicantStudyForViewDto> GetApplicantStudyForView(long id)
        {
            var applicantStudy = await _applicantStudyRepository.GetAsync(id);

            var output = new GetApplicantStudyForViewDto { ApplicantStudy = ObjectMapper.Map<ApplicantStudyDto>(applicantStudy) };

            if (output.ApplicantStudy.GraduationRateId != null)
            {
                var _lookupGraduationRate = await _lookup_graduationRateRepository.FirstOrDefaultAsync((int)output.ApplicantStudy.GraduationRateId);
                output.GraduationRateName = _lookupGraduationRate?.Name?.ToString();
            }

            if (output.ApplicantStudy.AcademicDegreeId != null)
            {
                var _lookupAcademicDegree = await _lookup_academicDegreeRepository.FirstOrDefaultAsync((int)output.ApplicantStudy.AcademicDegreeId);
                output.AcademicDegreeName = _lookupAcademicDegree?.Name?.ToString();
            }

            if (output.ApplicantStudy.SpecialtiesId != null)
            {
                var _lookupSpecialties = await _lookup_specialtiesRepository.FirstOrDefaultAsync((int)output.ApplicantStudy.SpecialtiesId);
                output.SpecialtiesName = _lookupSpecialties?.Name?.ToString();
            }

            if (output.ApplicantStudy.ApplicantId != null)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantStudy.ApplicantId);
                output.ApplicantFirstName = _lookupApplicant?.FirstName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies_Edit)]
        public async Task<GetApplicantStudyForEditOutput> GetApplicantStudyForEdit(EntityDto<long> input)
        {
            var applicantStudy = await _applicantStudyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicantStudyForEditOutput { ApplicantStudy = ObjectMapper.Map<CreateOrEditApplicantStudyDto>(applicantStudy) };

            if (output.ApplicantStudy.GraduationRateId != null)
            {
                var _lookupGraduationRate = await _lookup_graduationRateRepository.FirstOrDefaultAsync((int)output.ApplicantStudy.GraduationRateId);
                output.GraduationRateName = _lookupGraduationRate?.Name?.ToString();
            }

            if (output.ApplicantStudy.AcademicDegreeId != null)
            {
                var _lookupAcademicDegree = await _lookup_academicDegreeRepository.FirstOrDefaultAsync((int)output.ApplicantStudy.AcademicDegreeId);
                output.AcademicDegreeName = _lookupAcademicDegree?.Name?.ToString();
            }

            if (output.ApplicantStudy.SpecialtiesId != null)
            {
                var _lookupSpecialties = await _lookup_specialtiesRepository.FirstOrDefaultAsync((int)output.ApplicantStudy.SpecialtiesId);
                output.SpecialtiesName = _lookupSpecialties?.Name?.ToString();
            }

            if (output.ApplicantStudy.ApplicantId != null)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantStudy.ApplicantId);
                output.ApplicantFirstName = _lookupApplicant?.FirstName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApplicantStudyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies_Create)]
        protected virtual async Task Create(CreateOrEditApplicantStudyDto input)
        {
            var applicantStudy = ObjectMapper.Map<ApplicantStudy>(input);

            await _applicantStudyRepository.InsertAsync(applicantStudy);

        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies_Edit)]
        protected virtual async Task Update(CreateOrEditApplicantStudyDto input)
        {
            var applicantStudy = await _applicantStudyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, applicantStudy);

        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _applicantStudyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetApplicantStudiesToExcel(GetAllApplicantStudiesForExcelInput input)
        {

            var filteredApplicantStudies = _applicantStudyRepository.GetAll()
                        .Include(e => e.GraduationRateFk)
                        .Include(e => e.AcademicDegreeFk)
                        .Include(e => e.SpecialtiesFk)
                        .Include(e => e.ApplicantFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OtherSpecialty.Contains(input.Filter) || e.SecondSpecialty.Contains(input.Filter) || e.University.Contains(input.Filter) || e.GraduationCountry.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherSpecialtyFilter), e => e.OtherSpecialty.Contains(input.OtherSpecialtyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SecondSpecialtyFilter), e => e.SecondSpecialty.Contains(input.SecondSpecialtyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversityFilter), e => e.University.Contains(input.UniversityFilter))
                        .WhereIf(input.MinGraduationYearFilter != null, e => e.GraduationYear >= input.MinGraduationYearFilter)
                        .WhereIf(input.MaxGraduationYearFilter != null, e => e.GraduationYear <= input.MaxGraduationYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GraduationCountryFilter), e => e.GraduationCountry.Contains(input.GraduationCountryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GraduationRateNameFilter), e => e.GraduationRateFk != null && e.GraduationRateFk.Name == input.GraduationRateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AcademicDegreeNameFilter), e => e.AcademicDegreeFk != null && e.AcademicDegreeFk.Name == input.AcademicDegreeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SpecialtiesNameFilter), e => e.SpecialtiesFk != null && e.SpecialtiesFk.Name == input.SpecialtiesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFirstNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FirstName == input.ApplicantFirstNameFilter);

            var query = (from o in filteredApplicantStudies
                         join o1 in _lookup_graduationRateRepository.GetAll() on o.GraduationRateId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_academicDegreeRepository.GetAll() on o.AcademicDegreeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_specialtiesRepository.GetAll() on o.SpecialtiesId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetApplicantStudyForViewDto()
                         {
                             ApplicantStudy = new ApplicantStudyDto
                             {
                                 OtherSpecialty = o.OtherSpecialty,
                                 SecondSpecialty = o.SecondSpecialty,
                                 University = o.University,
                                 GraduationYear = o.GraduationYear,
                                 GraduationCountry = o.GraduationCountry,
                                 Id = o.Id
                             },
                             GraduationRateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             AcademicDegreeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             SpecialtiesName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             ApplicantFirstName = s4 == null || s4.FirstName == null ? "" : s4.FirstName.ToString()
                         });

            var applicantStudyListDtos = await query.ToListAsync();

            return _applicantStudiesExcelExporter.ExportToFile(applicantStudyListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies)]
        public async Task<List<ApplicantStudyGraduationRateLookupTableDto>> GetAllGraduationRateForTableDropdown()
        {
            return await _lookup_graduationRateRepository.GetAll()
                .Select(graduationRate => new ApplicantStudyGraduationRateLookupTableDto
                {
                    Id = graduationRate.Id,
                    DisplayName = graduationRate == null || graduationRate.Name == null ? "" : graduationRate.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies)]
        public async Task<List<ApplicantStudyAcademicDegreeLookupTableDto>> GetAllAcademicDegreeForTableDropdown()
        {
            return await _lookup_academicDegreeRepository.GetAll()
                .Select(academicDegree => new ApplicantStudyAcademicDegreeLookupTableDto
                {
                    Id = academicDegree.Id,
                    DisplayName = academicDegree == null || academicDegree.Name == null ? "" : academicDegree.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies)]
        public async Task<List<ApplicantStudySpecialtiesLookupTableDto>> GetAllSpecialtiesForTableDropdown()
        {
            return await _lookup_specialtiesRepository.GetAll()
                .Select(specialties => new ApplicantStudySpecialtiesLookupTableDto
                {
                    Id = specialties.Id,
                    DisplayName = specialties == null || specialties.Name == null ? "" : specialties.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStudies)]
        public async Task<List<ApplicantStudyApplicantLookupTableDto>> GetAllApplicantForTableDropdown()
        {
            return await _lookup_applicantRepository.GetAll()
                .Select(applicant => new ApplicantStudyApplicantLookupTableDto
                {
                    Id = applicant.Id,
                    DisplayName = applicant == null || applicant.FirstName == null ? "" : applicant.FirstName.ToString()
                }).ToListAsync();
        }

    }
}