﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAttachmentFileForEditOutput
    {
        public CreateOrEditAttachmentFileDto AttachmentFile { get; set; }

        public string AttachmentTypeArName { get; set; }

    }
}