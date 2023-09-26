using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.XRoad
{
    public interface IXRoadMappingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetXRoadMappingForViewDto>> GetAll(GetAllXRoadMappingsInput input);

        Task<GetXRoadMappingForViewDto> GetXRoadMappingForView(int id);

        Task<GetXRoadMappingForEditOutput> GetXRoadMappingForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditXRoadMappingDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetXRoadMappingsToExcel(GetAllXRoadMappingsForExcelInput input);

    }
}