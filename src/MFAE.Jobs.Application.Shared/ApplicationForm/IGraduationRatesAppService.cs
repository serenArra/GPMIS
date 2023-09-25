using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IGraduationRatesAppService : IApplicationService
    {
        Task<PagedResultDto<GetGraduationRateForViewDto>> GetAll(GetAllGraduationRatesInput input);

        Task<GetGraduationRateForViewDto> GetGraduationRateForView(int id);

        Task<GetGraduationRateForEditOutput> GetGraduationRateForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGraduationRateDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetGraduationRatesToExcel(GetAllGraduationRatesForExcelInput input);

    }
}