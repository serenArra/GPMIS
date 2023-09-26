using MFAE.Jobs.XRoad;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MFAE.Jobs.XRoad.Exporting;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.XRoad
{
    [AbpAuthorize(AppPermissions.Pages_XRoadMappings)]
    public class XRoadMappingsAppService : JobsAppServiceBase, IXRoadMappingsAppService
    {
        private readonly IRepository<XRoadMapping> _xrodeMappingRepository;
        private readonly IXRoadMappingsExcelExporter _xRoadMappingsExcelExporter;

        public XRoadMappingsAppService(IRepository<XRoadMapping> xRoadMappingRepository, IXRoadMappingsExcelExporter xRoadMappingsExcelExporter)
        {
            _xrodeMappingRepository = xRoadMappingRepository;
            _xRoadMappingsExcelExporter = xRoadMappingsExcelExporter;

        }

        public async Task<PagedResultDto<GetXRoadMappingForViewDto>> GetAll(GetAllXRoadMappingsInput input)
        {
            var lookupFilter = input.LookupFilter.HasValue
                         ? (XRoadLookupEnum)input.LookupFilter
                         : default;
            var serviceNameFilter = input.ServiceNameFilter.HasValue
                ? (XRoadServicesEnum)input.ServiceNameFilter
                : default;

            var filteredXrodeMappings = _xrodeMappingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter))
                        .WhereIf(input.LookupFilter.HasValue && input.LookupFilter > -1, e => e.Lookup == lookupFilter)
                        .WhereIf(input.ServiceNameFilter.HasValue && input.ServiceNameFilter > -1, e => e.ServiceName == serviceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.MinSystemIdFilter != null, e => e.SystemId >= input.MinSystemIdFilter)
                        .WhereIf(input.MaxSystemIdFilter != null, e => e.SystemId <= input.MaxSystemIdFilter);

            var pagedAndFilteredXrodeMappings = filteredXrodeMappings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var xrodeMappings = from o in pagedAndFilteredXrodeMappings
                                select new GetXRoadMappingForViewDto()
                                {
                                    XRoadMapping = new XRoadMappingDto
                                    {
                                        Lookup = o.Lookup,
                                        ServiceName = o.ServiceName,
                                        Code = o.Code,
                                        SystemId = o.SystemId,
                                        Id = o.Id
                                    }
                                };

            var totalCount = await filteredXrodeMappings.CountAsync();

            return new PagedResultDto<GetXRoadMappingForViewDto>(
                totalCount,
                await xrodeMappings.ToListAsync()
            );
        }

        public async Task<GetXRoadMappingForViewDto> GetXRoadMappingForView(int id)
        {
            var xrodeMapping = await _xrodeMappingRepository.GetAsync(id);

            var output = new GetXRoadMappingForViewDto { XRoadMapping = ObjectMapper.Map<XRoadMappingDto>(xrodeMapping) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadMappings_Edit)]
        public async Task<GetXRoadMappingForEditOutput> GetXRoadMappingForEdit(EntityDto input)
        {
            var xrodeMapping = await _xrodeMappingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetXRoadMappingForEditOutput { XRoadMapping = ObjectMapper.Map<CreateOrEditXRoadMappingDto>(xrodeMapping) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditXRoadMappingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_XRoadMappings_Create)]
        protected virtual async Task Create(CreateOrEditXRoadMappingDto input)
        {
            var xrodeMapping = ObjectMapper.Map<XRoadMapping>(input);

            await _xrodeMappingRepository.InsertAsync(xrodeMapping);

        }

        [AbpAuthorize(AppPermissions.Pages_XRoadMappings_Edit)]
        protected virtual async Task Update(CreateOrEditXRoadMappingDto input)
        {
            var xrodeMapping = await _xrodeMappingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, xrodeMapping);
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadMappings_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _xrodeMappingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetXRoadMappingsToExcel(GetAllXRoadMappingsForExcelInput input)
        {
            var lookupFilter = input.LookupFilter.HasValue
                        ? (XRoadLookupEnum)input.LookupFilter
                        : default;
            var serviceNameFilter = input.ServiceNameFilter.HasValue
                ? (XRoadServicesEnum)input.ServiceNameFilter
                : default;

            var filteredXRoadMappings = _xrodeMappingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter))
                        .WhereIf(input.LookupFilter.HasValue && input.LookupFilter > -1, e => e.Lookup == lookupFilter)
                        .WhereIf(input.ServiceNameFilter.HasValue && input.ServiceNameFilter > -1, e => e.ServiceName == serviceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.MinSystemIdFilter != null, e => e.SystemId >= input.MinSystemIdFilter)
                        .WhereIf(input.MaxSystemIdFilter != null, e => e.SystemId <= input.MaxSystemIdFilter);

            var query = (from o in filteredXRoadMappings
                         select new GetXRoadMappingForViewDto()
                         {
                             XRoadMapping = new XRoadMappingDto
                             {
                                 Lookup = o.Lookup,
                                 ServiceName = o.ServiceName,
                                 Code = o.Code,
                                 SystemId = o.SystemId,
                                 Id = o.Id
                             }
                         });

            var xRoadMappingListDtos = await query.ToListAsync();

            return _xRoadMappingsExcelExporter.ExportToFile(xRoadMappingListDtos);
        }

    }
}