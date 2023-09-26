using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantForEditOutput
    {
        public CreateOrEditApplicantDto Applicant { get; set; }

        public string IdentificationTypeName { get; set; }

        public string MaritalStatusName { get; set; }

        public string UserName { get; set; }

        public string ApplicantStatusDescription { get; set; }

        public string CountryName { get; set; }

        public string GovernorateName { get; set; }

        public string LocalityName { get; set; }

    }
}