using MFAE.Jobs.Attachments;

using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class AttachmentTypeDto : EntityDto
    {
        public string ArName { get; set; }

        public string EnName { get; set; }

        public int MaxSizeMB { get; set; }

        public string AllowedExtensions { get; set; }

        public int MaxAttachments { get; set; }

        public int MinRequiredAttachments { get; set; }

        public AttachmentTypeCategories Category { get; set; }

        public PrivacyFlag PrivacyFlag { get; set; }

        public int? EntityTypeId { get; set; }

        public int? GroupId { get; set; }

    }
}