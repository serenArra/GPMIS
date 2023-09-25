using System.Collections.Generic;
using MFAE.Jobs.Authorization.Permissions.Dto;

namespace MFAE.Jobs.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}