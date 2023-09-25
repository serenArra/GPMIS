using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}