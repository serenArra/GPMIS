using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.XRoad
{
    public interface IXRoadServiceAttributeMappingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetXRoadServiceAttributeMappingForViewDto>> GetAll(GetAllXRoadServiceAttributeMappingsInput input);

        Task<GetXRoadServiceAttributeMappingForViewDto> GetXRoadServiceAttributeMappingForView(int id);

        Task<XRoadServiceAttributeDto> GetXRoadServiceAttribute(int attributeId);

        Task<GetXRoadServiceAttributeMappingForEditOutput> GetXRoadServiceAttributeMappingForEdit(EntityDto input);
        Task<GetXRoadServiceAttributeMappingForEditOutput> GetXRoadServiceAttributeMappingForCreate(int attributeId);

        Task CreateOrEdit(CreateOrEditXRoadServiceAttributeMappingDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetXRoadServiceAttributeMappingsToExcel(GetAllXRoadServiceAttributeMappingsForExcelInput input);


        Task<PagedResultDto<XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto>> GetAllXRoadServiceAttributeForLookupTable(GetAllForLookupTableInput input);

    }
}