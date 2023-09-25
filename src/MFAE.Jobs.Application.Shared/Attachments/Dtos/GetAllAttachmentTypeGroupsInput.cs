using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAllAttachmentTypeGroupsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ArNameFilter { get; set; }

        public string EnNameFilter { get; set; }

    }
}