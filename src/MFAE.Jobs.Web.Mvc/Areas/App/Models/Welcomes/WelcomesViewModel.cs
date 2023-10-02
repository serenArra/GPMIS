using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Authorization.Users.Dto;
namespace MFAE.Jobs.Web.Areas.App.Models.Welcomes
{
    public class WelcomesViewModel
    {
        public UserListDto User { get; set; }

        public JobAdvertisementDto JobAdvertisement { get; set; }

        public string GovernorateName { get; set; }
    }
}
