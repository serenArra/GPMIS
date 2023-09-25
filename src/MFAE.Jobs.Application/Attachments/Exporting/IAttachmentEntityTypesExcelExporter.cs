using System.Collections.Generic;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Attachments.Exporting
{
    public interface IAttachmentEntityTypesExcelExporter
    {
        FileDto ExportToFile(List<GetAttachmentEntityTypeForViewDto> attachmentEntityTypes);
    }
}