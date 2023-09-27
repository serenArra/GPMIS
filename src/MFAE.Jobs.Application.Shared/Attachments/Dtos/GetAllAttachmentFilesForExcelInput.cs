using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAllAttachmentFilesForExcelInput
    {
        public string Filter { get; set; }

        public string PhysicalNameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string OriginalNameFilter { get; set; }

        public long? MaxSizeFilter { get; set; }
        public long? MinSizeFilter { get; set; }

        public string ObjectIdFilter { get; set; }

        public string PathFilter { get; set; }

        public int? MaxVersionFilter { get; set; }
        public int? MinVersionFilter { get; set; }

        public string FileTokenFilter { get; set; }

        public string TagFilter { get; set; }

        public string FilterKeyFilter { get; set; }

        public string AttachmentTypeArNameFilter { get; set; }

    }
}