using MFAE.Jobs.ApplicationForm;

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
    [AbpAuthorize(AppPermissions.Pages_ApplicantTrainings)]
    public class ApplicantTrainingsAppService : JobsAppServiceBase, IApplicantTrainingsAppService
    {
        private readonly IRepository<ApplicantTraining, long> _applicantTrainingRepository;
        private readonly IApplicantTrainingsExcelExporter _applicantTrainingsExcelExporter;
        private readonly IRepository<Applicant, long> _lookup_applicantRepository;

        public ApplicantTrainingsAppService(IRepository<ApplicantTraining, long> applicantTrainingRepository, IApplicantTrainingsExcelExporter applicantTrainingsExcelExporter, IRepository<Applicant, long> lookup_applicantRepository)
        {
            _applicantTrainingRepository = applicantTrainingRepository;
            _applicantTrainingsExcelExporter = applicantTrainingsExcelExporter;
            _lookup_applicantRepository = lookup_applicantRepository;

        }

        public async Task<PagedResultDto<GetApplicantTrainingForViewDto>> GetAll(GetAllApplicantTrainingsInput input)
        {
            var durationTypeFilter = input.DurationTypeFilter.HasValue
                        ? (DurationType)input.DurationTypeFilter
                        : default;

            var filteredApplicantTrainings = _applicantTrainingRepository.GetAll()
                        .Include(e => e.ApplicantFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Subject.Contains(input.Filter) || e.Location.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter), e => e.Subject.Contains(input.SubjectFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.Contains(input.LocationFilter))
                        .WhereIf(input.MinTrainingDateFilter != null, e => e.TrainingDate >= input.MinTrainingDateFilter)
                        .WhereIf(input.MaxTrainingDateFilter != null, e => e.TrainingDate <= input.MaxTrainingDateFilter)
                        .WhereIf(input.MinDurationFilter != null, e => e.Duration >= input.MinDurationFilter)
                        .WhereIf(input.MaxDurationFilter != null, e => e.Duration <= input.MaxDurationFilter)
                        .WhereIf(input.DurationTypeFilter.HasValue && input.DurationTypeFilter > -1, e => e.DurationType == durationTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFirstNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FirstName == input.ApplicantFirstNameFilter);

            var pagedAndFilteredApplicantTrainings = filteredApplicantTrainings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var applicantTrainings = from o in pagedAndFilteredApplicantTrainings
                                     join o1 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     select new
                                     {

                                         o.Subject,
                                         o.Location,
                                         o.TrainingDate,
                                         o.Duration,
                                         o.DurationType,
                                         Id = o.Id,
                                         ApplicantFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString()
                                     };

            var totalCount = await filteredApplicantTrainings.CountAsync();

            var dbList = await applicantTrainings.ToListAsync();
            var results = new List<GetApplicantTrainingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetApplicantTrainingForViewDto()
                {
                    ApplicantTraining = new ApplicantTrainingDto
                    {

                        Subject = o.Subject,
                        Location = o.Location,
                        TrainingDate = o.TrainingDate,
                        Duration = o.Duration,
                        DurationType = o.DurationType,
                        Id = o.Id,
                    },
                    ApplicantFirstName = o.ApplicantFirstName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetApplicantTrainingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetApplicantTrainingForViewDto> GetApplicantTrainingForView(long id)
        {
            var applicantTraining = await _applicantTrainingRepository.GetAsync(id);

            var output = new GetApplicantTrainingForViewDto { ApplicantTraining = ObjectMapper.Map<ApplicantTrainingDto>(applicantTraining) };

            if (output.ApplicantTraining.ApplicantId != null)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantTraining.ApplicantId);
                output.ApplicantFirstName = _lookupApplicant?.FirstName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantTrainings_Edit)]
        public async Task<GetApplicantTrainingForEditOutput> GetApplicantTrainingForEdit(EntityDto<long> input)
        {
            var applicantTraining = await _applicantTrainingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicantTrainingForEditOutput { ApplicantTraining = ObjectMapper.Map<CreateOrEditApplicantTrainingDto>(applicantTraining) };

            if (output.ApplicantTraining.ApplicantId != null)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantTraining.ApplicantId);
                output.ApplicantFirstName = _lookupApplicant?.FirstName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApplicantTrainingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ApplicantTrainings_Create)]
        protected virtual async Task Create(CreateOrEditApplicantTrainingDto input)
        {
            var applicantTraining = ObjectMapper.Map<ApplicantTraining>(input);

            await _applicantTrainingRepository.InsertAsync(applicantTraining);

        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantTrainings_Edit)]
        protected virtual async Task Update(CreateOrEditApplicantTrainingDto input)
        {
            var applicantTraining = await _applicantTrainingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, applicantTraining);

        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantTrainings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _applicantTrainingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetApplicantTrainingsToExcel(GetAllApplicantTrainingsForExcelInput input)
        {
            var durationTypeFilter = input.DurationTypeFilter.HasValue
                        ? (DurationType)input.DurationTypeFilter
                        : default;

            var filteredApplicantTrainings = _applicantTrainingRepository.GetAll()
                        .Include(e => e.ApplicantFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Subject.Contains(input.Filter) || e.Location.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter), e => e.Subject.Contains(input.SubjectFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.Contains(input.LocationFilter))
                        .WhereIf(input.MinTrainingDateFilter != null, e => e.TrainingDate >= input.MinTrainingDateFilter)
                        .WhereIf(input.MaxTrainingDateFilter != null, e => e.TrainingDate <= input.MaxTrainingDateFilter)
                        .WhereIf(input.MinDurationFilter != null, e => e.Duration >= input.MinDurationFilter)
                        .WhereIf(input.MaxDurationFilter != null, e => e.Duration <= input.MaxDurationFilter)
                        .WhereIf(input.DurationTypeFilter.HasValue && input.DurationTypeFilter > -1, e => e.DurationType == durationTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFirstNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FirstName == input.ApplicantFirstNameFilter);

            var query = (from o in filteredApplicantTrainings
                         join o1 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetApplicantTrainingForViewDto()
                         {
                             ApplicantTraining = new ApplicantTrainingDto
                             {
                                 Subject = o.Subject,
                                 Location = o.Location,
                                 TrainingDate = o.TrainingDate,
                                 Duration = o.Duration,
                                 DurationType = o.DurationType,
                                 Id = o.Id
                             },
                             ApplicantFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString()
                         });

            var applicantTrainingListDtos = await query.ToListAsync();

            return _applicantTrainingsExcelExporter.ExportToFile(applicantTrainingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantTrainings)]
        public async Task<List<ApplicantTrainingApplicantLookupTableDto>> GetAllApplicantForTableDropdown()
        {
            return await _lookup_applicantRepository.GetAll()
                .Select(applicant => new ApplicantTrainingApplicantLookupTableDto
                {
                    Id = applicant.Id,
                    DisplayName = applicant == null || applicant.FirstName == null ? "" : applicant.FirstName.ToString()
                }).ToListAsync();
        }

    }
}