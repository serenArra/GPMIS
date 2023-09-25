using System.Collections.Generic;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public interface IMaritalStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetMaritalStatusForViewDto> maritalStatuses);
    }
}