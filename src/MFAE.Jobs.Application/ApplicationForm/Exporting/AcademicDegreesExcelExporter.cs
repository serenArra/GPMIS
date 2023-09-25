using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class AcademicDegreesExcelExporter : MiniExcelExcelExporterBase, IAcademicDegreesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AcademicDegreesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAcademicDegreeForViewDto> academicDegrees)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var academicDegree in academicDegrees)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("ArName"), academicDegree.AcademicDegree.NameAr},
                    {L("EnName"), academicDegree.AcademicDegree.NameEn},
                    {L("Folder"), academicDegree.AcademicDegree.IsActive}
                });
            }

            return CreateExcelPackage("AcademicDegrees.xlsx", items);
        }
    }
}