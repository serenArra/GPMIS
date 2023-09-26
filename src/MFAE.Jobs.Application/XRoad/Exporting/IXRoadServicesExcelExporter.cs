using System.Collections.Generic;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.XRoad.Exporting
{
    public interface IXRoadServicesExcelExporter
    {
        FileDto ExportToFile(List<GetXRoadServiceForViewDto> xRoadServices);
    }
}