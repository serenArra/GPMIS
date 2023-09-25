using System.Collections.Generic;
using Abp;
using MFAE.Jobs.Chat.Dto;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
