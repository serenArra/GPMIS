using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class ConversationRatesExcelExporter : MiniExcelExcelExporterBase, IConversationRatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ConversationRatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetConversationRateForViewDto> conversationRates)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var conversationRate in conversationRates)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), conversationRate.ConversationRate.NameAr},
                    {L("NameEn"), conversationRate.ConversationRate.NameEn},
                    {L("IsActive"), conversationRate.ConversationRate.IsActive}                    
                });
            }

            return CreateExcelPackage("ConversationRates.xlsx", items);
        }
    }
}