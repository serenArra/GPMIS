using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class ApplicantStudiesExcelExporter : MiniExcelExcelExporterBase, IApplicantStudiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ApplicantStudiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetApplicantStudyForViewDto> applicantStudies)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var applicantStudy in applicantStudies)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("OtherSpecialty"), applicantStudy.ApplicantStudy.OtherSpecialty},
                    {L("SecondSpecialty"), applicantStudy.ApplicantStudy.SecondSpecialty},
                    {L("University"), applicantStudy.ApplicantStudy.University},
                    {L("GraduationYear"), applicantStudy.ApplicantStudy.GraduationYear},
                    {L("GraduationCountry"), applicantStudy.ApplicantStudy.GraduationCountry},
                    {L("GraduationRate"), applicantStudy.GraduationRateName},
                    {L("AcademicDegree"), applicantStudy.AcademicDegreeName},
                    {L("Specialties"), applicantStudy.SpecialtiesName},
                    {L("Applicant"), applicantStudy.ApplicantFirstName}
                });
            }

            return CreateExcelPackage("ApplicantStudies.xlsx", items);
        }
    }
}