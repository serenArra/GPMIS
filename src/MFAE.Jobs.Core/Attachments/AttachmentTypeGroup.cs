using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.Attachments
{
    [Table("AttachmentTypeGroups")]
    [Audited]
    public class AttachmentTypeGroup : FullAuditedEntity
    {

        [Required]
        [StringLength(AttachmentTypeGroupConsts.MaxArNameLength, MinimumLength = AttachmentTypeGroupConsts.MinArNameLength)]
        public virtual string ArName { get; set; }

        [Required]
        [StringLength(AttachmentTypeGroupConsts.MaxEnNameLength, MinimumLength = AttachmentTypeGroupConsts.MinEnNameLength)]
        public virtual string EnName { get; set; }

        public string Name
        {
            get
            {
                var name = "";

                if (CultureInfo.CurrentUICulture.Name != "ar" && !string.IsNullOrEmpty(this.EnName))
                {
                    name = this.EnName;
                }
                else
                {
                    name = this.ArName;
                }

                return name;
            }
        }

    }
}