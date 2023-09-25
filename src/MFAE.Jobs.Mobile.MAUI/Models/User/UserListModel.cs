using Abp.AutoMapper;
using MFAE.Jobs.Authorization.Users.Dto;

namespace MFAE.Jobs.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(UserListDto))]
    public class UserListModel : UserListDto
    {
        public string Photo { get; set; }

        public string FullName => Name + " " + Surname;
    }
}
