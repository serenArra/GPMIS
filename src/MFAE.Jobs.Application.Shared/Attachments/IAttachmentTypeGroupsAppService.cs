using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Attachments
{
    public interface IAttachmentTypeGroupsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAttachmentTypeGroupForViewDto>> GetAll(GetAllAttachmentTypeGroupsInput input);

        Task<GetAttachmentTypeGroupForViewDto> GetAttachmentTypeGroupForView(int id);

        Task<GetAttachmentTypeGroupForEditOutput> GetAttachmentTypeGroupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAttachmentTypeGroupDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAttachmentTypeGroupsToExcel(GetAllAttachmentTypeGroupsForExcelInput input);

    }
}