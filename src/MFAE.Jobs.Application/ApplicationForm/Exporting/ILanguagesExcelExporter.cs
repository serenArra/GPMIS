using System.Collections.Generic;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public interface ILanguagesExcelExporter
    {
        FileDto ExportToFile(List<GetAppLanguageForViewDto> languages);
    }
}