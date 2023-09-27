using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class AttachmentFileDto : EntityDto
    {
        public string PhysicalName { get; set; }

        public string Description { get; set; }

        public string OriginalName { get; set; }

        public long Size { get; set; }

        public string ObjectId { get; set; }

        public string Path { get; set; }

        public int Version { get; set; }

        public string FileToken { get; set; }

        public string Tag { get; set; }

        public string FilterKey { get; set; }

        public int AttachmentTypeId { get; set; }

    }
}