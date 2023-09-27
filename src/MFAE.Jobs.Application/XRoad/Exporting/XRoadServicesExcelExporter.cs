using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.XRoad.Exporting
{
    public class XRoadServicesExcelExporter : MiniExcelExcelExporterBase, IXRoadServicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public XRoadServicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetXRoadServiceForViewDto> xRoadServices)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var xRoadService in xRoadServices)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("Name"), xRoadService.XRoadService.Name},
                    {L("ProviderCode"), xRoadService.XRoadService.ProviderCode},
                    {L("ResultCodePath"), xRoadService.XRoadService.ResultCodePath},
                    {L("ActionName"), xRoadService.XRoadService.ActionName},
                    {L("SoapActionName"), xRoadService.XRoadService.SoapActionName},
                    {L("VersionNo"), xRoadService.XRoadService.VersionNo},
                    {L("ProducerCode"), xRoadService.XRoadService.ProducerCode},
                    {L("Description"), xRoadService.XRoadService.Description},
                    {L("Status"), xRoadService.XRoadService.Status},
                    {L("Code"), xRoadService.XRoadService.Code}
                });
            }

            return CreateExcelPackage("XRoadServices.xlsx", items);
        }
    }
}