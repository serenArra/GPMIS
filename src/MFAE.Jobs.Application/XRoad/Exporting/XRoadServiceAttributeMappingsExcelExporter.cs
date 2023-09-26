using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.XRoad.Exporting
{
    public class XRoadServiceAttributeMappingsExcelExporter : MiniExcelExcelExporterBase, IXRoadServiceAttributeMappingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public XRoadServiceAttributeMappingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetXRoadServiceAttributeMappingForViewDto> xRoadServiceAttributeMappings)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var xRoadServiceAttributeMapping in xRoadServiceAttributeMappings)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("ServiceAttributeType"), xRoadServiceAttributeMapping.XRoadServiceAttributeMapping.ServiceAttributeType},
                    {L("SourceValue"), xRoadServiceAttributeMapping.XRoadServiceAttributeMapping.SourceValue},
                    {L("DestinationValue"), xRoadServiceAttributeMapping.XRoadServiceAttributeMapping.DestinationValue},
                    {L("XRoadServiceAttribute"), xRoadServiceAttributeMapping.XRoadServiceAttributeName}
                });
            }

            return CreateExcelPackage("XRoadServiceAttributeMappings.xlsx", items);
        }
    }
}