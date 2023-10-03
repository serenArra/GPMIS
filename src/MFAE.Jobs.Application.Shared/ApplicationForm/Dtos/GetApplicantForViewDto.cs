using MFAE.Jobs.ApplicationForm.Enums;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantForViewDto
    {
        public ApplicantDto Applicant { get; set; }

        public string IdentificationTypeName { get; set; }

        public string MaritalStatusName { get; set; }

        public string UserName { get; set; }

        public string ApplicantStatusDescription { get; set; }

        public ApplicantStatusEnum CurrentStatus { get; set; }

        public ApplicantStatusEnum StatusID { get; set; }

        public string CountryName { get; set; }

        public string GovernorateName { get; set; }

        public string LocalityName { get; set; }

        public int applicantStudiesCount { get; set; }

        public int applicantTrainingCount { get; set; }

        public int applicantLanguageCount { get; set; }
    }
}