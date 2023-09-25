using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class ConversationsExcelExporter : MiniExcelExcelExporterBase, IConversationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ConversationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetConversationForViewDto> conversations)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var conversation in conversations)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), conversation.Conversation.NameAr},
                    {L("NameEn"), conversation.Conversation.NameEn},
                    {L("IsActive"), conversation.Conversation.IsActive}
                });
            }

            return CreateExcelPackage("Conversations.xlsx", items);
        }
    }
}