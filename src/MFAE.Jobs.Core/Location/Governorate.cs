using MFAE.Jobs.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.Location
{
    [Table("Governorates")]
    [Audited]
    public class Governorate : FullAuditedEntity
    {

        [Required]
        [StringLength(GovernorateConsts.MaxNameArLength, MinimumLength = GovernorateConsts.MinNameArLength)]
        public virtual string NameAr { get; set; }

        [StringLength(GovernorateConsts.MaxNameEnLength, MinimumLength = GovernorateConsts.MinNameEnLength)]
        public virtual string NameEn { get; set; }

        [StringLength(GovernorateConsts.MaxUniversalCodeLength, MinimumLength = GovernorateConsts.MinUniversalCodeLength)]
        public virtual string UniversalCode { get; set; }

        public virtual int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public string Name
        {
            get
            {
                var name = "";

                if (CultureInfo.CurrentUICulture.Name != "ar" && !string.IsNullOrEmpty(this.NameEn))
                {
                    name = this.NameEn;
                }
                else
                {
                    name = this.NameAr;
                }

                return name;
            }
        }

    }
}