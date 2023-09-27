using System.Collections.Generic;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.XRoad.Exporting
{
    public interface IXRoadServiceErrorsExcelExporter
    {
        FileDto ExportToFile(List<GetXRoadServiceErrorForViewDto> xRoadServiceErrors);
    }
}