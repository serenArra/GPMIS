using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.XRoad.Exporting
{
    public class XRoadServiceErrorsExcelExporter : MiniExcelExcelExporterBase, IXRoadServiceErrorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public XRoadServiceErrorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetXRoadServiceErrorForViewDto> xRoadServiceErrors)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var xRoadServiceError in xRoadServiceErrors)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("ErrorCode"), xRoadServiceError.XRoadServiceError.ErrorCode},
                    {L("ErrorMessageAr"), xRoadServiceError.XRoadServiceError.ErrorMessageAr},
                    {L("ErrorMessageEn"), xRoadServiceError.XRoadServiceError.ErrorMessageEn},
                    {L("XRoadService"), xRoadServiceError.XRoadServiceName}
                });
            }

            return CreateExcelPackage("XRoadServiceErrors.xlsx", items);
        }
    }
}