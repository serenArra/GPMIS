using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.Location
{
    public interface ILocalitiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetLocalityForViewDto>> GetAll(GetAllLocalitiesInput input);

        Task<GetLocalityForViewDto> GetLocalityForView(int id);

        Task<GetLocalityForEditOutput> GetLocalityForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLocalityDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetLocalitiesToExcel(GetAllLocalitiesForExcelInput input);

        Task<List<LocalityGovernorateLookupTableDto>> GetAllGovernorateForTableDropdown();

    }
}