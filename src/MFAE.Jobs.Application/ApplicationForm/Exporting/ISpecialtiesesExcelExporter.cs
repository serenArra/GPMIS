using System.Collections.Generic;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public interface ISpecialtiesesExcelExporter
    {
        FileDto ExportToFile(List<GetSpecialtiesForViewDto> specialtieses);
    }
}