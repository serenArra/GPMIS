using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}