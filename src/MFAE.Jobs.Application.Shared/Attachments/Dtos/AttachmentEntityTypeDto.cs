using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class AttachmentEntityTypeDto : EntityDto
    {
        public string ArName { get; set; }

        public string EnName { get; set; }

        public string Folder { get; set; }

        public int? ParentTypeId { get; set; }

    }
}