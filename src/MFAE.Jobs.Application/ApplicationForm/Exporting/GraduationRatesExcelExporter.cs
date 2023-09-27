using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class GraduationRatesExcelExporter : MiniExcelExcelExporterBase, IGraduationRatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GraduationRatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGraduationRateForViewDto> graduationRates)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var graduationRate in graduationRates)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), graduationRate.GraduationRate.NameAr},
                    {L("NameEn"), graduationRate.GraduationRate.NameEn},
                    {L("IsActive"), graduationRate.GraduationRate.IsActive}
                });
            }

            return CreateExcelPackage("GraduationRates.xlsx", items);
        }
    }
}