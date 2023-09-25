using Abp.AutoMapper;
using MFAE.Jobs.Authorization.Roles.Dto;
using MFAE.Jobs.Web.Areas.App.Models.Common;

namespace MFAE.Jobs.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}