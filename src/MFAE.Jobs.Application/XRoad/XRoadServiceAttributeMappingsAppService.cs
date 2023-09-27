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
    [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings)]
    public class XRoadServiceAttributeMappingsAppService : JobsAppServiceBase, IXRoadServiceAttributeMappingsAppService
    {
        private readonly IRepository<XRoadServiceAttributeMapping> _xRoadServiceAttributeMappingRepository;
        private readonly IXRoadServiceAttributeMappingsExcelExporter _xRoadServiceAttributeMappingsExcelExporter;
        private readonly IRepository<XRoadServiceAttribute, int> _lookup_xRoadServiceAttributeRepository;

        public XRoadServiceAttributeMappingsAppService(IRepository<XRoadServiceAttributeMapping> xRoadServiceAttributeMappingRepository, IXRoadServiceAttributeMappingsExcelExporter xRoadServiceAttributeMappingsExcelExporter, IRepository<XRoadServiceAttribute, int> lookup_xRoadServiceAttributeRepository)
        {
            _xRoadServiceAttributeMappingRepository = xRoadServiceAttributeMappingRepository;
            _xRoadServiceAttributeMappingsExcelExporter = xRoadServiceAttributeMappingsExcelExporter;
            _lookup_xRoadServiceAttributeRepository = lookup_xRoadServiceAttributeRepository;

        }

        public async Task<PagedResultDto<GetXRoadServiceAttributeMappingForViewDto>> GetAll(GetAllXRoadServiceAttributeMappingsInput input)
        {
            var serviceAttributeTypeFilter = input.ServiceAttributeTypeFilter.HasValue
                        ? (XRoadAttributeTypeEnum)input.ServiceAttributeTypeFilter
                        : default;

            var filteredXRoadServiceAttributeMappings = _xRoadServiceAttributeMappingRepository.GetAll()
                        .Include(e => e.AttributeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SourceValue.Contains(input.Filter) || e.DestinationValue.Contains(input.Filter))
                        .WhereIf(input.ServiceAttributeTypeFilter.HasValue && input.ServiceAttributeTypeFilter > -1, e => e.ServiceAttributeType == serviceAttributeTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SourceValueFilter), e => e.SourceValue == input.SourceValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DestinationValueFilter), e => e.DestinationValue == input.DestinationValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XRoadServiceAttributeNameFilter), e => e.AttributeFk != null && e.AttributeFk.Name == input.XRoadServiceAttributeNameFilter)
                        .WhereIf((input.AttributeId != null), e => e.AttributeFk != null && e.AttributeFk.Id == input.AttributeId);


            var pagedAndFilteredXRoadServiceAttributeMappings = filteredXRoadServiceAttributeMappings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var xRoadServiceAttributeMappings = from o in pagedAndFilteredXRoadServiceAttributeMappings
                                                join o1 in _lookup_xRoadServiceAttributeRepository.GetAll() on o.AttributeID equals o1.Id into j1
                                                from s1 in j1.DefaultIfEmpty()

                                                select new GetXRoadServiceAttributeMappingForViewDto()
                                                {
                                                    XRoadServiceAttributeMapping = new XRoadServiceAttributeMappingDto
                                                    {
                                                        ServiceAttributeType = o.ServiceAttributeType,
                                                        SourceValue = o.SourceValue,
                                                        DestinationValue = o.DestinationValue,
                                                        Id = o.Id
                                                    },
                                                    XRoadServiceAttributeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                                };

            var totalCount = await filteredXRoadServiceAttributeMappings.CountAsync();

            return new PagedResultDto<GetXRoadServiceAttributeMappingForViewDto>(
                totalCount,
                await xRoadServiceAttributeMappings.ToListAsync()
            );

        }

        public async Task<XRoadServiceAttributeDto> GetXRoadServiceAttribute(int attributeId)
        {
            var _lookupXRoadServiceAttribute = await _lookup_xRoadServiceAttributeRepository.FirstOrDefaultAsync(attributeId);
            var result = ObjectMapper.Map<XRoadServiceAttributeDto>(_lookupXRoadServiceAttribute);
            return result;
        }

        public async Task<GetXRoadServiceAttributeMappingForViewDto> GetXRoadServiceAttributeMappingForView(int id)
        {
            var xRoadServiceAttributeMapping = await _xRoadServiceAttributeMappingRepository.GetAsync(id);

            var output = new GetXRoadServiceAttributeMappingForViewDto { XRoadServiceAttributeMapping = ObjectMapper.Map<XRoadServiceAttributeMappingDto>(xRoadServiceAttributeMapping) };

            if (output.XRoadServiceAttributeMapping.AttributeID != null)
            {
                var _lookupXRoadServiceAttribute = await _lookup_xRoadServiceAttributeRepository.FirstOrDefaultAsync((int)output.XRoadServiceAttributeMapping.AttributeID);
                output.XRoadServiceAttributeName = _lookupXRoadServiceAttribute?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Edit)]
        public async Task<GetXRoadServiceAttributeMappingForEditOutput> GetXRoadServiceAttributeMappingForEdit(EntityDto input)
        {
            var xRoadServiceAttributeMapping = await _xRoadServiceAttributeMappingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetXRoadServiceAttributeMappingForEditOutput { XRoadServiceAttributeMapping = ObjectMapper.Map<CreateOrEditXRoadServiceAttributeMappingDto>(xRoadServiceAttributeMapping) };

            if (output.XRoadServiceAttributeMapping.AttributeID != null)
            {
                var _lookupXRoadServiceAttribute = await _lookup_xRoadServiceAttributeRepository.FirstOrDefaultAsync((int)output.XRoadServiceAttributeMapping.AttributeID);
                output.XRoadServiceAttributeName = _lookupXRoadServiceAttribute?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Create)]
        public async Task<GetXRoadServiceAttributeMappingForEditOutput> GetXRoadServiceAttributeMappingForCreate(int attributeId)
        {


            var output = new GetXRoadServiceAttributeMappingForEditOutput
            {
                XRoadServiceAttributeMapping = new CreateOrEditXRoadServiceAttributeMappingDto()
            };
            output.XRoadServiceAttributeMapping.AttributeID = attributeId;
            var _lookupXRoadServiceAttribute = await _lookup_xRoadServiceAttributeRepository.FirstOrDefaultAsync(attributeId);
            output.XRoadServiceAttributeName = _lookupXRoadServiceAttribute?.Name?.ToString();


            return output;
        }

        public async Task CreateOrEdit(CreateOrEditXRoadServiceAttributeMappingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Create)]
        protected virtual async Task Create(CreateOrEditXRoadServiceAttributeMappingDto input)
        {
            var xRoadServiceAttributeMapping = ObjectMapper.Map<XRoadServiceAttributeMapping>(input);

            await _xRoadServiceAttributeMappingRepository.InsertAsync(xRoadServiceAttributeMapping);

        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Edit)]
        protected virtual async Task Update(CreateOrEditXRoadServiceAttributeMappingDto input)
        {
            var xRoadServiceAttributeMapping = await _xRoadServiceAttributeMappingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, xRoadServiceAttributeMapping);

        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _xRoadServiceAttributeMappingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetXRoadServiceAttributeMappingsToExcel(GetAllXRoadServiceAttributeMappingsForExcelInput input)
        {
            var serviceAttributeTypeFilter = input.ServiceAttributeTypeFilter.HasValue
                        ? (XRoadAttributeTypeEnum)input.ServiceAttributeTypeFilter
                        : default;

            var filteredXRoadServiceAttributeMappings = _xRoadServiceAttributeMappingRepository.GetAll()
                        .Include(e => e.AttributeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SourceValue.Contains(input.Filter) || e.DestinationValue.Contains(input.Filter))
                        .WhereIf(input.ServiceAttributeTypeFilter.HasValue && input.ServiceAttributeTypeFilter > -1, e => e.ServiceAttributeType == serviceAttributeTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SourceValueFilter), e => e.SourceValue.Contains(input.SourceValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DestinationValueFilter), e => e.DestinationValue.Contains(input.DestinationValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XRoadServiceAttributeNameFilter), e => e.AttributeFk != null && e.AttributeFk.Name == input.XRoadServiceAttributeNameFilter);

            var query = (from o in filteredXRoadServiceAttributeMappings
                         join o1 in _lookup_xRoadServiceAttributeRepository.GetAll() on o.AttributeID equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetXRoadServiceAttributeMappingForViewDto()
                         {
                             XRoadServiceAttributeMapping = new XRoadServiceAttributeMappingDto
                             {
                                 ServiceAttributeType = o.ServiceAttributeType,
                                 SourceValue = o.SourceValue,
                                 DestinationValue = o.DestinationValue,
                                 Id = o.Id
                             },
                             XRoadServiceAttributeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var xRoadServiceAttributeMappingListDtos = await query.ToListAsync();

            return _xRoadServiceAttributeMappingsExcelExporter.ExportToFile(xRoadServiceAttributeMappingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings)]
        public async Task<List<XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto>> GetAllXRoadServiceAttributeForTableDropdown()
        {
            return await _lookup_xRoadServiceAttributeRepository.GetAll()
                .Select(xRoadServiceAttribute => new XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto
                {
                    Id = xRoadServiceAttribute.Id,
                    DisplayName = xRoadServiceAttribute == null || xRoadServiceAttribute.Name == null ? "" : xRoadServiceAttribute.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings)]
        public async Task<PagedResultDto<XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto>> GetAllXRoadServiceAttributeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_xRoadServiceAttributeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var xRoadServiceAttributeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto>();
            foreach (var xRoadServiceAttribute in xRoadServiceAttributeList)
            {
                lookupTableDtoList.Add(new XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto
                {
                    Id = xRoadServiceAttribute.Id,
                    DisplayName = xRoadServiceAttribute.Name?.ToString()
                });
            }

            return new PagedResultDto<XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}