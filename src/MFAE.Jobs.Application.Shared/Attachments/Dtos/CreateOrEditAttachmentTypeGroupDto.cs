using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class CreateOrEditAttachmentTypeGroupDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AttachmentTypeGroupConsts.MaxArNameLength, MinimumLength = AttachmentTypeGroupConsts.MinArNameLength)]
        public string ArName { get; set; }

        [Required]
        [StringLength(AttachmentTypeGroupConsts.MaxEnNameLength, MinimumLength = AttachmentTypeGroupConsts.MinEnNameLength)]
        public string EnName { get; set; }

    }
}