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
    [AbpAuthorize(AppPermissions.Pages_MaritalStatuses)]
    public class MaritalStatusesAppService : JobsAppServiceBase, IMaritalStatusesAppService
    {
        private readonly IRepository<MaritalStatus> _maritalStatusRepository;
        private readonly IMaritalStatusesExcelExporter _maritalStatusesExcelExporter;

        public MaritalStatusesAppService(IRepository<MaritalStatus> maritalStatusRepository, IMaritalStatusesExcelExporter maritalStatusesExcelExporter)
        {
            _maritalStatusRepository = maritalStatusRepository;
            _maritalStatusesExcelExporter = maritalStatusesExcelExporter;

        }

        public async Task<PagedResultDto<GetMaritalStatusForViewDto>> GetAll(GetAllMaritalStatusesInput input)
        {

            var filteredMaritalStatuses = _maritalStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.IsActive.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsActiveFilter), e => e.IsActive.Contains(input.IsActiveFilter));

            var pagedAndFilteredMaritalStatuses = filteredMaritalStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var maritalStatuses = from o in pagedAndFilteredMaritalStatuses
                                  select new
                                  {

                                      o.NameAr,
                                      o.NameEn,
                                      o.IsActive,
                                      Id = o.Id
                                  };

            var totalCount = await filteredMaritalStatuses.CountAsync();

            var dbList = await maritalStatuses.ToListAsync();
            var results = new List<GetMaritalStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMaritalStatusForViewDto()
                {
                    MaritalStatus = new MaritalStatusDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMaritalStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMaritalStatusForViewDto> GetMaritalStatusForView(int id)
        {
            var maritalStatus = await _maritalStatusRepository.GetAsync(id);

            var output = new GetMaritalStatusForViewDto { MaritalStatus = ObjectMapper.Map<MaritalStatusDto>(maritalStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MaritalStatuses_Edit)]
        public async Task<GetMaritalStatusForEditOutput> GetMaritalStatusForEdit(EntityDto input)
        {
            var maritalStatus = await _maritalStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMaritalStatusForEditOutput { MaritalStatus = ObjectMapper.Map<CreateOrEditMaritalStatusDto>(maritalStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMaritalStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MaritalStatuses_Create)]
        protected virtual async Task Create(CreateOrEditMaritalStatusDto input)
        {
            var maritalStatus = ObjectMapper.Map<MaritalStatus>(input);

            await _maritalStatusRepository.InsertAsync(maritalStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_MaritalStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditMaritalStatusDto input)
        {
            var maritalStatus = await _maritalStatusRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, maritalStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_MaritalStatuses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _maritalStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMaritalStatusesToExcel(GetAllMaritalStatusesForExcelInput input)
        {

            var filteredMaritalStatuses = _maritalStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.IsActive.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsActiveFilter), e => e.IsActive.Contains(input.IsActiveFilter));

            var query = (from o in filteredMaritalStatuses
                         select new GetMaritalStatusForViewDto()
                         {
                             MaritalStatus = new MaritalStatusDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var maritalStatusListDtos = await query.ToListAsync();

            return _maritalStatusesExcelExporter.ExportToFile(maritalStatusListDtos);
        }

    }
}