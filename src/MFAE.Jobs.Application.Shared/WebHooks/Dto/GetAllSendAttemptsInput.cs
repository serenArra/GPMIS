using MFAE.Jobs.Dto;

namespace MFAE.Jobs.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
