using System.Collections.Generic;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.XRoad.Exporting
{
    public interface IXRoadServiceAttributeMappingsExcelExporter
    {
        FileDto ExportToFile(List<GetXRoadServiceAttributeMappingForViewDto> xRoadServiceAttributeMappings);
    }
}