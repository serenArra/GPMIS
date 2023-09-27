using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class ApplicantStatusesExcelExporter : MiniExcelExcelExporterBase, IApplicantStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ApplicantStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetApplicantStatusForViewDto> applicantStatuses)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var applicantStatus in applicantStatuses)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("Status"), applicantStatus.ApplicantStatus.Status},
                    {L("Description"), applicantStatus.ApplicantStatus.Description},
                    {L("Applicant"), applicantStatus.ApplicantFullName}
                });
            }

            return CreateExcelPackage("ApplicantStatuses.xlsx", items);
        }
    }
}