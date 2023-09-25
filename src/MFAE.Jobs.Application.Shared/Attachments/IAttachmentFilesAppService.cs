using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.Attachments
{
    public interface IAttachmentFilesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAttachmentFileForViewDto>> GetAll(GetAllAttachmentFilesInput input);

        Task<GetAttachmentFileForViewDto> GetAttachmentFileForView(int id);

        Task<GetAttachmentFileForEditOutput> GetAttachmentFileForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAttachmentFileDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAttachmentFilesToExcel(GetAllAttachmentFilesForExcelInput input);

        Task<List<AttachmentFileAttachmentTypeLookupTableDto>> GetAllAttachmentTypeForTableDropdown();

    }
}