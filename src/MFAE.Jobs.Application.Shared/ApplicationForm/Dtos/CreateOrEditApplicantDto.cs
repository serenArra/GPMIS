using MFAE.Jobs.ApplicationForm.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditApplicantDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ApplicantConsts.MaxDocumentNoLength, MinimumLength = ApplicantConsts.MinDocumentNoLength)]
        public string DocumentNo { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFirstNameLength, MinimumLength = ApplicantConsts.MinFirstNameLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFatherNameLength, MinimumLength = ApplicantConsts.MinFatherNameLength)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxGrandFatherNameLength, MinimumLength = ApplicantConsts.MinGrandFatherNameLength)]
        public string GrandFatherName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFamilyNameLength, MinimumLength = ApplicantConsts.MinFamilyNameLength)]
        public string FamilyName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFirstNameEnLength, MinimumLength = ApplicantConsts.MinFirstNameEnLength)]
        public string FirstNameEn { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFatherNameEnLength, MinimumLength = ApplicantConsts.MinFatherNameEnLength)]
        public string FatherNameEn { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxGrandFatherNameEnLength, MinimumLength = ApplicantConsts.MinGrandFatherNameEnLength)]
        public string GrandFatherNameEn { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFamilyNameEnLength, MinimumLength = ApplicantConsts.MinFamilyNameEnLength)]
        public string FamilyNameEn { get; set; }

        public DateTime BirthDate { get; set; }

        [StringLength(ApplicantConsts.MaxBirthPlaceLength, MinimumLength = ApplicantConsts.MinBirthPlaceLength)]
        public string BirthPlace { get; set; }

        public int? NumberOfChildren { get; set; }

        [StringLength(ApplicantConsts.MaxAddressLength, MinimumLength = ApplicantConsts.MinAddressLength)]
        public string Address { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxMobileLength, MinimumLength = ApplicantConsts.MinMobileLength)]
        public string Mobile { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxEmailLength, MinimumLength = ApplicantConsts.MinEmailLength)]
        public string Email { get; set; }

        public bool IsLocked { get; set; }

        public Gender Gender { get; set; }

        public int IdentificationTypeId { get; set; }

        public int MaritalStatusId { get; set; }

        public long? LockedBy { get; set; }

        public long? CurrentStatusId { get; set; }

    }
}