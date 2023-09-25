using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAllAttachmentEntityTypesForExcelInput
    {
        public string Filter { get; set; }

        public string ArNameFilter { get; set; }

        public string EnNameFilter { get; set; }

        public string FolderFilter { get; set; }

        public int? MaxParentTypeIdFilter { get; set; }
        public int? MinParentTypeIdFilter { get; set; }

    }
}