using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}