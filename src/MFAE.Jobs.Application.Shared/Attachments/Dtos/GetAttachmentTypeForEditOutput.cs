using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAttachmentTypeForEditOutput
    {
        public CreateOrEditAttachmentTypeDto AttachmentType { get; set; }

        public string AttachmentEntityTypeName { get; set; }

        public string AttachmentTypeGroupName { get; set; }

    }
}