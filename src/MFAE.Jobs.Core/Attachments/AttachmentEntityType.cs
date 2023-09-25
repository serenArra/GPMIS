using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Globalization;

namespace MFAE.Jobs.Attachments
{
    [Table("AttachmentEntityTypes")]
    [Audited]
    public class AttachmentEntityType : FullAuditedEntity
    {

        [Required]
        [StringLength(AttachmentEntityTypeConsts.MaxArNameLength, MinimumLength = AttachmentEntityTypeConsts.MinArNameLength)]
        public virtual string ArName { get; set; }

        [Required]
        [StringLength(AttachmentEntityTypeConsts.MaxEnNameLength, MinimumLength = AttachmentEntityTypeConsts.MinEnNameLength)]
        public virtual string EnName { get; set; }

        [Required]
        [StringLength(AttachmentEntityTypeConsts.MaxFolderLength, MinimumLength = AttachmentEntityTypeConsts.MinFolderLength)]
        public virtual string Folder { get; set; }

        public virtual int? ParentTypeId { get; set; }

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