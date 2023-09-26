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
    [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributes)]
    public class XRoadServiceAttributesAppService : JobsAppServiceBase, IXRoadServiceAttributesAppService
    {
        private readonly IRepository<XRoadServiceAttribute> _xRoadServiceAttributeRepository;
        private readonly IXRoadServiceAttributesExcelExporter _xRoadServiceAttributesExcelExporter;
        private readonly IRepository<XRoadService, int> _lookup_xRoadServiceRepository;

        public XRoadServiceAttributesAppService(IRepository<XRoadServiceAttribute> xRoadServiceAttributeRepository, IXRoadServiceAttributesExcelExporter xRoadServiceAttributesExcelExporter, IRepository<XRoadService, int> lookup_xRoadServiceRepository)
        {
            _xRoadServiceAttributeRepository = xRoadServiceAttributeRepository;
            _xRoadServiceAttributesExcelExporter = xRoadServiceAttributesExcelExporter;
            _lookup_xRoadServiceRepository = lookup_xRoadServiceRepository;

        }

        public async Task<PagedResultDto<GetXRoadServiceAttributeForViewDto>> GetAll(GetAllXRoadServiceAttributesInput input)
        {
            var serviceAttributeTypeFilter = input.ServiceAttributeTypeFilter.HasValue
                        ? (XRoadServiceAttributeTypeEnum)input.ServiceAttributeTypeFilter
                        : default;

            var filteredXRoadServiceAttributes = _xRoadServiceAttributeRepository.GetAll()
                        .Include(e => e.XRoadServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AttributeCode.Contains(input.Filter) || e.XMLPath.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.FormatTransition.Contains(input.Filter))
                        .WhereIf(input.ServiceAttributeTypeFilter.HasValue && input.ServiceAttributeTypeFilter > -1, e => e.ServiceAttributeType == serviceAttributeTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttributeCodeFilter), e => e.AttributeCode == input.AttributeCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XMLPathFilter), e => e.XMLPath == input.XMLPathFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormatTransitionFilter), e => e.FormatTransition == input.FormatTransitionFilter)
                        .WhereIf((input.XRoadServiceId != null), e => e.XRoadServiceFk != null && e.XRoadServiceFk.Id == input.XRoadServiceId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XRoadServiceNameFilter), e => e.XRoadServiceFk != null && e.XRoadServiceFk.Name == input.XRoadServiceNameFilter);

            var pagedAndFilteredXRoadServiceAttributes = filteredXRoadServiceAttributes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var xRoadServiceAttributes = from o in pagedAndFilteredXRoadServiceAttributes
                                         join o1 in _lookup_xRoadServiceRepository.GetAll() on o.XRoadServiceID equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         select new GetXRoadServiceAttributeForViewDto()
                                         {
                                             XRoadServiceAttribute = new XRoadServiceAttributeDto
                                             {
                                                 ServiceAttributeType = o.ServiceAttributeType,
                                                 AttributeCode = o.AttributeCode,
                                                 XMLPath = o.XMLPath,
                                                 Name = o.Name,
                                                 Description = o.Description,
                                                 FormatTransition = o.FormatTransition,
                                                 Id = o.Id
                                             },
                                             XRoadServiceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                         };

            var totalCount = await filteredXRoadServiceAttributes.CountAsync();

            return new PagedResultDto<GetXRoadServiceAttributeForViewDto>(
                totalCount,
                await xRoadServiceAttributes.ToListAsync()
            );

        }

        public async Task<GetXRoadServiceAttributeForViewDto> GetXRoadServiceAttributeForView(int id)
        {
            var xRoadServiceAttribute = await _xRoadServiceAttributeRepository.GetAsync(id);

            var output = new GetXRoadServiceAttributeForViewDto { XRoadServiceAttribute = ObjectMapper.Map<XRoadServiceAttributeDto>(xRoadServiceAttribute) };

            if (output.XRoadServiceAttribute.XRoadServiceID != null)
            {
                var _lookupXRoadService = await _lookup_xRoadServiceRepository.FirstOrDefaultAsync((int)output.XRoadServiceAttribute.XRoadServiceID);
                output.XRoadServiceName = _lookupXRoadService?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Edit)]
        public async Task<GetXRoadServiceAttributeForEditOutput> GetXRoadServiceAttributeForEdit(EntityDto input)
        {
            var xRoadServiceAttribute = await _xRoadServiceAttributeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetXRoadServiceAttributeForEditOutput { XRoadServiceAttribute = ObjectMapper.Map<CreateOrEditXRoadServiceAttributeDto>(xRoadServiceAttribute) };

            if (output.XRoadServiceAttribute.XRoadServiceID != null)
            {
                var _lookupXRoadService = await _lookup_xRoadServiceRepository.FirstOrDefaultAsync((int)output.XRoadServiceAttribute.XRoadServiceID);
                output.XRoadServiceName = _lookupXRoadService?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Create)]
        public async Task<GetXRoadServiceAttributeForEditOutput> GetXRoadServiceAttributeForCreate(int serviceId)
        {
            var output = new GetXRoadServiceAttributeForEditOutput
            {
                XRoadServiceAttribute = new CreateOrEditXRoadServiceAttributeDto()
            };

            output.XRoadServiceAttribute.XRoadServiceID = serviceId;
            var _lookupXRoadService = await _lookup_xRoadServiceRepository.FirstOrDefaultAsync(serviceId);
            output.XRoadServiceName = _lookupXRoadService?.Name?.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditXRoadServiceAttributeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Create)]
        protected virtual async Task Create(CreateOrEditXRoadServiceAttributeDto input)
        {
            var xRoadServiceAttribute = ObjectMapper.Map<XRoadServiceAttribute>(input);

            await _xRoadServiceAttributeRepository.InsertAsync(xRoadServiceAttribute);

        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Edit)]
        protected virtual async Task Update(CreateOrEditXRoadServiceAttributeDto input)
        {
            var xRoadServiceAttribute = await _xRoadServiceAttributeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, xRoadServiceAttribute);

        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _xRoadServiceAttributeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetXRoadServiceAttributesToExcel(GetAllXRoadServiceAttributesForExcelInput input)
        {
            var serviceAttributeTypeFilter = input.ServiceAttributeTypeFilter.HasValue
                        ? (XRoadServiceAttributeTypeEnum)input.ServiceAttributeTypeFilter
                        : default;

            var filteredXRoadServiceAttributes = _xRoadServiceAttributeRepository.GetAll()
                        .Include(e => e.XRoadServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AttributeCode.Contains(input.Filter) || e.XMLPath.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.FormatTransition.Contains(input.Filter))
                        .WhereIf(input.ServiceAttributeTypeFilter.HasValue && input.ServiceAttributeTypeFilter > -1, e => e.ServiceAttributeType == serviceAttributeTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttributeCodeFilter), e => e.AttributeCode.Contains(input.AttributeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XMLPathFilter), e => e.XMLPath.Contains(input.XMLPathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormatTransitionFilter), e => e.FormatTransition.Contains(input.FormatTransitionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XRoadServiceNameFilter), e => e.XRoadServiceFk != null && e.XRoadServiceFk.Name == input.XRoadServiceNameFilter);

            var query = (from o in filteredXRoadServiceAttributes
                         join o1 in _lookup_xRoadServiceRepository.GetAll() on o.XRoadServiceID equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetXRoadServiceAttributeForViewDto()
                         {
                             XRoadServiceAttribute = new XRoadServiceAttributeDto
                             {
                                 ServiceAttributeType = o.ServiceAttributeType,
                                 AttributeCode = o.AttributeCode,
                                 XMLPath = o.XMLPath,
                                 Name = o.Name,
                                 Description = o.Description,
                                 FormatTransition = o.FormatTransition,
                                 Id = o.Id
                             },
                             XRoadServiceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var xRoadServiceAttributeListDtos = await query.ToListAsync();

            return _xRoadServiceAttributesExcelExporter.ExportToFile(xRoadServiceAttributeListDtos);
        }        

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributes)]
        public async Task<PagedResultDto<XRoadServiceAttributeXRoadServiceLookupTableDto>> GetAllXRoadServiceForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_xRoadServiceRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var xRoadServiceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<XRoadServiceAttributeXRoadServiceLookupTableDto>();
            foreach (var xRoadService in xRoadServiceList)
            {
                lookupTableDtoList.Add(new XRoadServiceAttributeXRoadServiceLookupTableDto
                {
                    Id = xRoadService.Id,
                    DisplayName = xRoadService.Name?.ToString()
                });
            }

            return new PagedResultDto<XRoadServiceAttributeXRoadServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}