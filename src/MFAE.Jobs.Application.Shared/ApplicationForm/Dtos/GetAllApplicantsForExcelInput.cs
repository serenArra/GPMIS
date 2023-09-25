using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllApplicantsForExcelInput
    {
        public string Filter { get; set; }

        public string DocumentNoFilter { get; set; }

        public string FirstNameFilter { get; set; }

        public string FatherNameFilter { get; set; }

        public string GrandFatherNameFilter { get; set; }

        public string FamilyNameFilter { get; set; }

        public string FirstNameEnFilter { get; set; }

        public string FatherNameEnFilter { get; set; }

        public string GrandFatherNameEnFilter { get; set; }

        public string FamilyNameEnFilter { get; set; }

        public DateTime? MaxBirthDateFilter { get; set; }
        public DateTime? MinBirthDateFilter { get; set; }

        public string BirthPlaceFilter { get; set; }

        public int? MaxNumberOfChildrenFilter { get; set; }
        public int? MinNumberOfChildrenFilter { get; set; }

        public string AddressFilter { get; set; }

        public string MobileFilter { get; set; }

        public string EmailFilter { get; set; }

        public int? IsLockedFilter { get; set; }

        public int? GenderFilter { get; set; }

        public string IdentificationTypeNameFilter { get; set; }

        public string MaritalStatusNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string ApplicantStatusDescriptionFilter { get; set; }

    }
}