using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}