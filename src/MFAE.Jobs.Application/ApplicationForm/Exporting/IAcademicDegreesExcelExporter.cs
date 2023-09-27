using System.Collections.Generic;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public interface IAcademicDegreesExcelExporter
    {
        FileDto ExportToFile(List<GetAcademicDegreeForViewDto> academicDegrees);
    }
}