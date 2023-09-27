using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.Location.Exporting
{
    public class LocalitiesExcelExporter : MiniExcelExcelExporterBase, ILocalitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LocalitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLocalityForViewDto> localities)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var locality in localities)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), locality.Locality.NameAr},
                    {L("NameEn"), locality.Locality.NameEn},
                    {L("UniversalCode"), locality.Locality.UniversalCode},
                    {L("Governorate"), locality.GovernorateName}
                });
            }

            return CreateExcelPackage("Localities.xlsx", items);
        }
    }
}