using Abp.AutoMapper;
using MFAE.Jobs.Authorization.Users.Dto;

namespace MFAE.Jobs.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
