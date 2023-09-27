

using MFAE.Jobs.ApplicationForm.Dtos;

namespace MFAE.Jobs.Authorization.Users.Dto
{
    public class WelcomeUserDto
    {
        public UserListDto User { get; set; }

        public JobAdvertisementDto JobAdvertisement { get; set; }
    }
}
