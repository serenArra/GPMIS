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
    [AbpAuthorize(AppPermissions.Pages_ApplicantStatuses)]
    public class ApplicantStatusesAppService : JobsAppServiceBase, IApplicantStatusesAppService
    {
        private readonly IRepository<ApplicantStatus, long> _applicantStatusRepository;
        private readonly IApplicantStatusesExcelExporter _applicantStatusesExcelExporter;
        private readonly IRepository<Applicant, long> _lookup_applicantRepository;

        public ApplicantStatusesAppService(IRepository<ApplicantStatus, long> applicantStatusRepository, IApplicantStatusesExcelExporter applicantStatusesExcelExporter, IRepository<Applicant, long> lookup_applicantRepository)
        {
            _applicantStatusRepository = applicantStatusRepository;
            _applicantStatusesExcelExporter = applicantStatusesExcelExporter;
            _lookup_applicantRepository = lookup_applicantRepository;

        }

        public async Task<PagedResultDto<GetApplicantStatusForViewDto>> GetAll(GetAllApplicantStatusesInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (ApplicantStatusEnum)input.StatusFilter
                        : default;

            var filteredApplicantStatuses = _applicantStatusRepository.GetAll()
                        .Include(e => e.ApplicantFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFullNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FullName == input.ApplicantFullNameFilter);

            var pagedAndFilteredApplicantStatuses = filteredApplicantStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var applicantStatuses = from o in pagedAndFilteredApplicantStatuses
                                    join o1 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    select new
                                    {

                                        o.Status,
                                        o.Description,
                                        Id = o.Id,
                                        ApplicantFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString()
                                    };

            var totalCount = await filteredApplicantStatuses.CountAsync();

            var dbList = await applicantStatuses.ToListAsync();
            var results = new List<GetApplicantStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetApplicantStatusForViewDto()
                {
                    ApplicantStatus = new ApplicantStatusDto
                    {

                        Status = o.Status,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    ApplicantFullName = o.ApplicantFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetApplicantStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetApplicantStatusForViewDto> GetApplicantStatusForView(long id)
        {
            var applicantStatus = await _applicantStatusRepository.GetAsync(id);

            var output = new GetApplicantStatusForViewDto { ApplicantStatus = ObjectMapper.Map<ApplicantStatusDto>(applicantStatus) };

            if (output.ApplicantStatus.ApplicantId != null)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantStatus.ApplicantId);
                output.ApplicantFullName = _lookupApplicant?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStatuses_Edit)]
        public async Task<GetApplicantStatusForEditOutput> GetApplicantStatusForEdit(EntityDto<long> input)
        {
            var applicantStatus = await _applicantStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicantStatusForEditOutput { ApplicantStatus = ObjectMapper.Map<CreateOrEditApplicantStatusDto>(applicantStatus) };

            if (output.ApplicantStatus.ApplicantId > 0)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantStatus.ApplicantId);
                output.ApplicantFullName = _lookupApplicant?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApplicantStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ApplicantStatuses_Create)]
        protected virtual async Task Create(CreateOrEditApplicantStatusDto input)
        {
            var applicantStatus = ObjectMapper.Map<ApplicantStatus>(input);

            await _applicantStatusRepository.InsertAsync(applicantStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditApplicantStatusDto input)
        {
            var applicantStatus = await _applicantStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, applicantStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _applicantStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetApplicantStatusesToExcel(GetAllApplicantStatusesForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (ApplicantStatusEnum)input.StatusFilter
                        : default;

            var filteredApplicantStatuses = _applicantStatusRepository.GetAll()
                        .Include(e => e.ApplicantFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFullNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FullName == input.ApplicantFullNameFilter);

            var query = (from o in filteredApplicantStatuses
                         join o1 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetApplicantStatusForViewDto()
                         {
                             ApplicantStatus = new ApplicantStatusDto
                             {
                                 Status = o.Status,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             ApplicantFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString()
                         });

            var applicantStatusListDtos = await query.ToListAsync();

            return _applicantStatusesExcelExporter.ExportToFile(applicantStatusListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantStatuses)]
        public async Task<List<ApplicantStatusApplicantLookupTableDto>> GetAllApplicantForTableDropdown()
        {
            return await _lookup_applicantRepository.GetAll()
                .Select(applicant => new ApplicantStatusApplicantLookupTableDto
                {
                    Id = applicant.Id,
                    DisplayName = applicant == null || applicant.FullName == null ? "" : applicant.FullName.ToString()
                }).ToListAsync();
        }

    }
}