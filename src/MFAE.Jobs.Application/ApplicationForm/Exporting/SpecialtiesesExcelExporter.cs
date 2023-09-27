using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class SpecialtiesesExcelExporter : MiniExcelExcelExporterBase, ISpecialtiesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SpecialtiesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSpecialtiesForViewDto> specialtieses)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var specialties in specialtieses)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), specialties.Specialties.NameAr},
                    {L("NameEn"), specialties.Specialties.NameEn},
                    {L("IsActive"), specialties.Specialties.IsActive}
                });
            }

            return CreateExcelPackage("Specialtieses.xlsx", items);
        }
    }
}