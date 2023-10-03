using MFAE.Jobs.ApplicationForm.Enums;

using System;
using Abp.Application.Services.Dto;
using System.Globalization;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class ApplicantDto : EntityDto<long>
    {
        public string DocumentNo { get; set; }

        public string FirstName { get; set; }

        public string FatherName { get; set; }

        public string GrandFatherName { get; set; }

        public string FamilyName { get; set; }

        public string FirstNameEn { get; set; }

        public string FatherNameEn { get; set; }

        public string GrandFatherNameEn { get; set; }

        public string FamilyNameEn { get; set; }

        public DateTime BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public int? NumberOfChildren { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public bool IsLocked { get; set; }

        public Gender Gender { get; set; }

        public int IdentificationTypeId { get; set; }

        public int MaritalStatusId { get; set; }

        public long? LockedBy { get; set; }

        public long? CurrentStatusId { get; set; }

        public int CountryId { get; set; }

        public int? GovernorateId { get; set; }

        public int? LocalityId { get; set; }

        public string FullName
        {
            get
            {
                var name = "";

                if (CultureInfo.CurrentUICulture.Name != "ar" && !string.IsNullOrEmpty(this.FirstNameEn))
                {
                    name = this.FirstNameEn + " " + this.FatherNameEn + " " + this.GrandFatherNameEn + " " + this.FamilyNameEn;
                }
                else
                {
                    name = this.FirstNameEn + " " + this.FatherNameEn + " " + this.GrandFatherNameEn + " " + this.FamilyNameEn;
                }

                return name;
            }
        }

    }
}