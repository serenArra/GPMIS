using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("AppLanguages")]
    [Audited]
    public class AppLanguage : FullAuditedEntity
    {

        [Required]
        [StringLength(LanguageConsts.MaxNameArLength, MinimumLength = LanguageConsts.MinNameArLength)]
        public virtual string NameAr { get; set; }

        [Required]
        [StringLength(LanguageConsts.MaxNameEnLength, MinimumLength = LanguageConsts.MinNameEnLength)]
        public virtual string NameEn { get; set; }

        public virtual bool IsActive { get; set; }

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