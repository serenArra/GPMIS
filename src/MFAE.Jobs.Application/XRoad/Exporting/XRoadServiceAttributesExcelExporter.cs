using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.XRoad.Exporting
{
    public class XRoadServiceAttributesExcelExporter : MiniExcelExcelExporterBase, IXRoadServiceAttributesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public XRoadServiceAttributesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetXRoadServiceAttributeForViewDto> xRoadServiceAttributes)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var xRoadServiceAttribute in xRoadServiceAttributes)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("ServiceAttributeType"), xRoadServiceAttribute.XRoadServiceAttribute.ServiceAttributeType},
                    {L("AttributeCode"), xRoadServiceAttribute.XRoadServiceAttribute.AttributeCode},
                    {L("XMLPath"), xRoadServiceAttribute.XRoadServiceAttribute.XMLPath},
                    {L("Name"), xRoadServiceAttribute.XRoadServiceAttribute.Name},
                    {L("Description"), xRoadServiceAttribute.XRoadServiceAttribute.Description},
                    {L("FormatTransition"), xRoadServiceAttribute.XRoadServiceAttribute.FormatTransition},
                    {L("XRoadService"), xRoadServiceAttribute.XRoadServiceAttribute}
                });
            }

            return CreateExcelPackage("XRoadServiceAttributes.xlsx", items);
        }
    }
}