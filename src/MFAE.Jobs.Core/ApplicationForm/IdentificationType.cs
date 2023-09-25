using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("IdentificationTypes")]
    [Audited]
    public class IdentificationType : FullAuditedEntity
    {

        [Required]
        [StringLength(IdentificationTypeConsts.MaxNameArLength, MinimumLength = IdentificationTypeConsts.MinNameArLength)]
        public virtual string NameAr { get; set; }

        [Required]
        [StringLength(IdentificationTypeConsts.MaxNameEnLength, MinimumLength = IdentificationTypeConsts.MinNameEnLength)]
        public virtual string NameEn { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual bool IsDefault { get; set; }

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