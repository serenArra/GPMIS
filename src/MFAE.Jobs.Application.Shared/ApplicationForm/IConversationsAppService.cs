using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IConversationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetConversationForViewDto>> GetAll(GetAllConversationsInput input);

        Task<GetConversationForViewDto> GetConversationForView(int id);

        Task<GetConversationForEditOutput> GetConversationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditConversationDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetConversationsToExcel(GetAllConversationsForExcelInput input);

    }
}