using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.XRoad
{
    public interface IXRoadServiceErrorsAppService : IApplicationService
    {
        Task<PagedResultDto<GetXRoadServiceErrorForViewDto>> GetAll(GetAllXRoadServiceErrorsInput input);

        Task<GetXRoadServiceErrorForViewDto> GetXRoadServiceErrorForView(int id);

        Task<GetXRoadServiceErrorForEditOutput> GetXRoadServiceErrorForEdit(EntityDto input);
        Task<GetXRoadServiceErrorForEditOutput> GetXRoadServiceErrorForCreate(int serviceId);

        Task CreateOrEdit(CreateOrEditXRoadServiceErrorDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetXRoadServiceErrorsToExcel(GetAllXRoadServiceErrorsForExcelInput input);

        Task<PagedResultDto<XRoadServiceErrorXRoadServiceLookupTableDto>> GetAllXRoadServiceForLookupTable(GetAllForLookupTableInput input);

    }
}