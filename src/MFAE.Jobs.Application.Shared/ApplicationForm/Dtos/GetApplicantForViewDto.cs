﻿namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantForViewDto
    {
        public ApplicantDto Applicant { get; set; }

        public string IdentificationTypeName { get; set; }

        public string MaritalStatusName { get; set; }

        public string UserName { get; set; }

        public string ApplicantStatusDescription { get; set; }

    }
}