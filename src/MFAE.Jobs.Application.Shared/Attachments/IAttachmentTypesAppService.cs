using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace MFAE.Jobs.Attachments
{
    public interface IAttachmentTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAttachmentTypeForViewDto>> GetAll(GetAllAttachmentTypesInput input);

        Task<GetAttachmentTypeForViewDto> GetAttachmentTypeForView(int id);

        Task<GetAttachmentTypeForEditOutput> GetAttachmentTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAttachmentTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAttachmentTypesToExcel(GetAllAttachmentTypesForExcelInput input);

        Task<List<AttachmentTypeAttachmentEntityTypeLookupTableDto>> GetAllAttachmentEntityTypeForTableDropdown();

        Task<List<AttachmentTypeAttachmentTypeGroupLookupTableDto>> GetAllAttachmentTypeGroupForTableDropdown();

    }
}