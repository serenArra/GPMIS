using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.XRoad.Exporting
{
    public class XRoadMappingsExcelExporter : MiniExcelExcelExporterBase, IXRoadMappingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public XRoadMappingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetXRoadMappingForViewDto> xRoadMappings)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var xRoadMapping in xRoadMappings)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("Lookup"), xRoadMapping.XRoadMapping.Lookup},
                    {L("ServiceName"), xRoadMapping.XRoadMapping.ServiceName},
                    {L("Code"), xRoadMapping.XRoadMapping.Code},
                    {L("SystemId"), xRoadMapping.XRoadMapping.SystemId}
                });
            }

            return CreateExcelPackage("XRoadMappings.xlsx", items);
        }
    }
}