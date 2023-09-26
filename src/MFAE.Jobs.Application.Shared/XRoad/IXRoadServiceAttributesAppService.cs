using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.XRoad
{
    public interface IXRoadServiceAttributesAppService : IApplicationService
    {
        Task<PagedResultDto<GetXRoadServiceAttributeForViewDto>> GetAll(GetAllXRoadServiceAttributesInput input);

        Task<GetXRoadServiceAttributeForViewDto> GetXRoadServiceAttributeForView(int id);

        Task<GetXRoadServiceAttributeForEditOutput> GetXRoadServiceAttributeForEdit(EntityDto input);
        Task<GetXRoadServiceAttributeForEditOutput> GetXRoadServiceAttributeForCreate(int serviceId);

        Task CreateOrEdit(CreateOrEditXRoadServiceAttributeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetXRoadServiceAttributesToExcel(GetAllXRoadServiceAttributesForExcelInput input);


        Task<PagedResultDto<XRoadServiceAttributeXRoadServiceLookupTableDto>> GetAllXRoadServiceForLookupTable(GetAllForLookupTableInput input);

    }
}