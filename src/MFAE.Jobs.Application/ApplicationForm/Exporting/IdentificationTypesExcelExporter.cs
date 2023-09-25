using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class IdentificationTypesExcelExporter : MiniExcelExcelExporterBase, IIdentificationTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public IdentificationTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetIdentificationTypeForViewDto> identificationTypes)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var identificationType in identificationTypes)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), identificationType.IdentificationType.NameAr},
                    {L("NameEn"), identificationType.IdentificationType.NameEn},
                    {L("IsActive"), identificationType.IdentificationType.IsActive},
                    {L("IsDefault"), identificationType.IdentificationType.IsDefault}
                });
            }

            return CreateExcelPackage("IdentificationTypes.xlsx", items);
        }
    }
}