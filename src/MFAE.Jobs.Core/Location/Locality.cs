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
    [Table("Localities")]
    [Audited]
    public class Locality : FullAuditedEntity
    {

        [Required]
        [StringLength(LocalityConsts.MaxNameArLength, MinimumLength = LocalityConsts.MinNameArLength)]
        public virtual string NameAr { get; set; }

        [Required]
        [StringLength(LocalityConsts.MaxNameEnLength, MinimumLength = LocalityConsts.MinNameEnLength)]
        public virtual string NameEn { get; set; }

        [StringLength(LocalityConsts.MaxUniversalCodeLength, MinimumLength = LocalityConsts.MinUniversalCodeLength)]
        public virtual string UniversalCode { get; set; }

        public virtual int GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate GovernorateFk { get; set; }

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