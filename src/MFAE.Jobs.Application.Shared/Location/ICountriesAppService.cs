using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Location
{
    public interface ICountriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input);

        Task<GetCountryForViewDto> GetCountryForView(int id);

        Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCountryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input);

    }
}