using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("Specialtieses")]
    [Audited]
    public class Specialties : FullAuditedEntity
    {

        [Required]
        [StringLength(SpecialtiesConsts.MaxNameArLength, MinimumLength = SpecialtiesConsts.MinNameArLength)]
        public virtual string NameAr { get; set; }

        [Required]
        [StringLength(SpecialtiesConsts.MaxNameEnLength, MinimumLength = SpecialtiesConsts.MinNameEnLength)]
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