using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class MaritalStatusesExcelExporter : MiniExcelExcelExporterBase, IMaritalStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MaritalStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMaritalStatusForViewDto> maritalStatuses)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var maritalStatus in maritalStatuses)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), maritalStatus.MaritalStatus.NameAr},
                    {L("NameEn"), maritalStatus.MaritalStatus.NameEn},
                    {L("IsActive"), maritalStatus.MaritalStatus.IsActive}
                });
            }

            return CreateExcelPackage("MaritalStatuses.xlsx", items);
        }
    }
}