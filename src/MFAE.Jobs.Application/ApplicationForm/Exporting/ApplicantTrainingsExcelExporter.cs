using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class ApplicantTrainingsExcelExporter : MiniExcelExcelExporterBase, IApplicantTrainingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ApplicantTrainingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetApplicantTrainingForViewDto> applicantTrainings)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var applicantTraining in applicantTrainings)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("Subject"), applicantTraining.ApplicantTraining.Subject},
                    {L("Location"), applicantTraining.ApplicantTraining.Location},
                    {L("TrainingDate"), applicantTraining.ApplicantTraining.TrainingDate},
                    {L("Duration"), applicantTraining.ApplicantTraining.Duration},
                    {L("DurationType"), applicantTraining.ApplicantTraining.DurationType},
                    {L("Applicant"), applicantTraining.ApplicantFirstName}
                });
            }

            return CreateExcelPackage("ApplicantTrainings.xlsx", items);
        }
    }
}