using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IConversationRatesAppService : IApplicationService
    {
        Task<PagedResultDto<GetConversationRateForViewDto>> GetAll(GetAllConversationRatesInput input);

        Task<GetConversationRateForViewDto> GetConversationRateForView(int id);

        Task<GetConversationRateForEditOutput> GetConversationRateForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditConversationRateDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetConversationRatesToExcel(GetAllConversationRatesForExcelInput input);

    }
}