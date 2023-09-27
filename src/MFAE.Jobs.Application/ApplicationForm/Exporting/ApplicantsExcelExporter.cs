using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class ApplicantsExcelExporter : MiniExcelExcelExporterBase, IApplicantsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ApplicantsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetApplicantForViewDto> applicants)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var applicant in applicants)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("DocumentNo"), applicant.Applicant.DocumentNo},
                    {L("FirstName"), applicant.Applicant.FirstName},
                    {L("FatherName"), applicant.Applicant.FatherName},
                    {L("GrandFatherName"), applicant.Applicant.GrandFatherName},
                    {L("FamilyName"), applicant.Applicant.FamilyName},
                    {L("FirstNameEn"), applicant.Applicant.FirstNameEn},
                    {L("FatherNameEn"), applicant.Applicant.FatherNameEn},
                    {L("GrandFatherNameEn"), applicant.Applicant.GrandFatherNameEn},
                    {L("FamilyNameEn"), applicant.Applicant.FamilyNameEn},
                    {L("BirthDate"), applicant.Applicant.BirthDate},
                    {L("BirthPlace"), applicant.Applicant.BirthPlace},
                    {L("NumberOfChildren"), applicant.Applicant.NumberOfChildren},
                    {L("Address"), applicant.Applicant.Address},
                    {L("Mobile"), applicant.Applicant.Mobile},
                    {L("Email"), applicant.Applicant.Email},
                    {L("IsLocked"), applicant.Applicant.IsLocked},
                    {L("Gender"), applicant.Applicant.Gender},
                    {L("IdentificationType"), applicant.Applicant.IdentificationTypeId},
                    {L("MaritalStatus"), applicant.Applicant.MaritalStatusId},
                    {L("ApplicantStatus"), applicant.ApplicantStatusDescription}
                    
                });
            }

            return CreateExcelPackage("Applicants.xlsx", items);
        }
    }
}