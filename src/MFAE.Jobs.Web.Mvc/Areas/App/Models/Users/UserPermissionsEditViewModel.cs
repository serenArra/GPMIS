using Abp.AutoMapper;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Authorization.Users.Dto;
using MFAE.Jobs.Web.Areas.App.Models.Common;

namespace MFAE.Jobs.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}