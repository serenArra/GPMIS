using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IAppLanguagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAppLanguageForViewDto>> GetAll(GetAllAppLanguagesInput input);

        Task<GetAppLanguageForViewDto> GetLanguageForView(int id);

        Task<GetAppLanguageForEditOutput> GetLanguageForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAppLanguageDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetLanguagesToExcel(GetAllLanguagesForExcelInput input);

    }
}