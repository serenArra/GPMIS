using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Location.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}