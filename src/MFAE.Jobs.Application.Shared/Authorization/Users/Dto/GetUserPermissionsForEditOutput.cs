using System.Collections.Generic;
using MFAE.Jobs.Authorization.Permissions.Dto;

namespace MFAE.Jobs.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}