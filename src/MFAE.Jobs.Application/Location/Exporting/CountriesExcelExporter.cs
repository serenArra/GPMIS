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
    public class CountriesExcelExporter : MiniExcelExcelExporterBase, ICountriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CountriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCountryForViewDto> countries)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var country in countries)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("IsoNumeric"), country.Country.IsoNumeric},
                    {L("IsoAlpha"), country.Country.IsoAlpha},
                    {L("NameAr"), country.Country.NameAr},
                    {L("NameEn"), country.Country.NameEn},
                    {L("UniversalCode"), country.Country.UniversalCode}
                });
            }

            return CreateExcelPackage("Countries.xlsx", items);            
        }
    }
}