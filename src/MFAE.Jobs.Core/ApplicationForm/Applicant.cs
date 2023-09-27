using MFAE.Jobs.ApplicationForm.Enums;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("Applicants")]
    [Audited]
    public class Applicant : FullAuditedEntity<long>
    {

        [Required]
        [StringLength(ApplicantConsts.MaxDocumentNoLength, MinimumLength = ApplicantConsts.MinDocumentNoLength)]
        public virtual string DocumentNo { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFirstNameLength, MinimumLength = ApplicantConsts.MinFirstNameLength)]
        public virtual string FirstName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFatherNameLength, MinimumLength = ApplicantConsts.MinFatherNameLength)]
        public virtual string FatherName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxGrandFatherNameLength, MinimumLength = ApplicantConsts.MinGrandFatherNameLength)]
        public virtual string GrandFatherName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFamilyNameLength, MinimumLength = ApplicantConsts.MinFamilyNameLength)]
        public virtual string FamilyName { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFirstNameEnLength, MinimumLength = ApplicantConsts.MinFirstNameEnLength)]
        public virtual string FirstNameEn { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFatherNameEnLength, MinimumLength = ApplicantConsts.MinFatherNameEnLength)]
        public virtual string FatherNameEn { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxGrandFatherNameEnLength, MinimumLength = ApplicantConsts.MinGrandFatherNameEnLength)]
        public virtual string GrandFatherNameEn { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxFamilyNameEnLength, MinimumLength = ApplicantConsts.MinFamilyNameEnLength)]
        public virtual string FamilyNameEn { get; set; }

        public virtual DateTime BirthDate { get; set; }

        [StringLength(ApplicantConsts.MaxBirthPlaceLength, MinimumLength = ApplicantConsts.MinBirthPlaceLength)]
        public virtual string BirthPlace { get; set; }

        public virtual int? NumberOfChildren { get; set; }

        [StringLength(ApplicantConsts.MaxAddressLength, MinimumLength = ApplicantConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxMobileLength, MinimumLength = ApplicantConsts.MinMobileLength)]
        public virtual string Mobile { get; set; }

        [Required]
        [StringLength(ApplicantConsts.MaxEmailLength, MinimumLength = ApplicantConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        public virtual bool IsLocked { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual int IdentificationTypeId { get; set; }

        [ForeignKey("IdentificationTypeId")]
        public IdentificationType IdentificationTypeFk { get; set; }

        public virtual int MaritalStatusId { get; set; }

        [ForeignKey("MaritalStatusId")]
        public MaritalStatus MaritalStatusFk { get; set; }

        public virtual long? LockedBy { get; set; }

        [ForeignKey("LockedBy")]
        public User LockedByFk { get; set; }

        public virtual long? CurrentStatusId { get; set; }

        [ForeignKey("CurrentStatusId")]
        public ApplicantStatus CurrentStatusFk { get; set; }

        public virtual int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual int? GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate GovernorateFk { get; set; }

        public virtual int? LocalityId { get; set; }

        [ForeignKey("LocalityId")]
        public Locality LocalityFk { get; set; }

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