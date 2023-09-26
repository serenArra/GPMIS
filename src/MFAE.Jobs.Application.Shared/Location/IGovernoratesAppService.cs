using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.Location
{
    public interface IGovernoratesAppService : IApplicationService
    {
        Task<PagedResultDto<GetGovernorateForViewDto>> GetAll(GetAllGovernoratesInput input);

        Task<GetGovernorateForViewDto> GetGovernorateForView(int id);

        Task<GetGovernorateForEditOutput> GetGovernorateForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGovernorateDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetGovernoratesToExcel(GetAllGovernoratesForExcelInput input);

        Task<List<GovernorateCountryLookupTableDto>> GetAllCountryForTableDropdown();

    }
}