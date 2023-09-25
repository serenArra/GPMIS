using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Notifications.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}