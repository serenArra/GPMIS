using System.Collections.Generic;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Attachments.Exporting
{
    public interface IAttachmentFilesExcelExporter
    {
        FileDto ExportToFile(List<GetAttachmentFileForViewDto> attachmentFiles);
    }
}