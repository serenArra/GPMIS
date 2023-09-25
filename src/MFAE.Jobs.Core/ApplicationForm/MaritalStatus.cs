using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("MaritalStatuses")]
    [Audited]
    public class MaritalStatus : FullAuditedEntity
    {

        [Required]
        [StringLength(MaritalStatusConsts.MaxNameArLength, MinimumLength = MaritalStatusConsts.MinNameArLength)]
        public virtual string NameAr { get; set; }

        [Required]
        [StringLength(MaritalStatusConsts.MaxNameEnLength, MinimumLength = MaritalStatusConsts.MinNameEnLength)]
        public virtual string NameEn { get; set; }

        public virtual string IsActive { get; set; }

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