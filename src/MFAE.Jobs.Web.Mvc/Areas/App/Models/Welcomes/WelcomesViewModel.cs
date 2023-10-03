using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Authorization.Users.Dto;
namespace MFAE.Jobs.Web.Areas.App.Models.Welcomes
{
    public class WelcomesViewModel
    {
        public UserListDto User { get; set; }

        public JobAdvertisementDto JobAdvertisement { get; set; }

        public GetApplicantForViewDto ApplicantForViewDto { get; set; }

        public bool IsApplicant { get; set; }

        public string ApplicantAddress { get; set; }
    }
}
