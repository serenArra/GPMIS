using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;
using MFAE.Jobs.ApplicationForm;

namespace MFAE.Jobs.Location.Exporting
{
    public class GovernoratesExcelExporter : MiniExcelExcelExporterBase, IGovernoratesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GovernoratesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGovernorateForViewDto> governorates)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var governorate in governorates)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), governorate.Governorate.NameAr},
                    {L("NameEn"), governorate.Governorate.NameEn},
                    {L("UniversalCode"), governorate.Governorate.UniversalCode},
                    {L("Country"), governorate.CountryName}
                });
            }

            return CreateExcelPackage("Governorates.xlsx", items);
        }
    }
}