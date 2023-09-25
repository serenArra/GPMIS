using System.Collections.Generic;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Attachments.Exporting
{
    public interface IAttachmentTypeGroupsExcelExporter
    {
        FileDto ExportToFile(List<GetAttachmentTypeGroupForViewDto> attachmentTypeGroups);
    }
}