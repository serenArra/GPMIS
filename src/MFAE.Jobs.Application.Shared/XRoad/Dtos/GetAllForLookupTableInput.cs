using Abp.Application.Services.Dto;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}