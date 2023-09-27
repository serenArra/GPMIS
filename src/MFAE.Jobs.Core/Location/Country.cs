using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.Location
{
    [Table("Countries")]
    [Audited]
    public class Country : FullAuditedEntity
    {

        [Required]
        [StringLength(CountryConsts.MaxIsoNumericLength, MinimumLength = CountryConsts.MinIsoNumericLength)]
        public virtual string IsoNumeric { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxIsoAlphaLength, MinimumLength = CountryConsts.MinIsoAlphaLength)]
        public virtual string IsoAlpha { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxNameArLength, MinimumLength = CountryConsts.MinNameArLength)]
        public virtual string NameAr { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxNameEnLength, MinimumLength = CountryConsts.MinNameEnLength)]
        public virtual string NameEn { get; set; }

        [StringLength(CountryConsts.MaxUniversalCodeLength, MinimumLength = CountryConsts.MinUniversalCodeLength)]
        public virtual string UniversalCode { get; set; }

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