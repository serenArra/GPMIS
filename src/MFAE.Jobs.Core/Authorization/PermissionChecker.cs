using Abp.Authorization;
using MFAE.Jobs.Authorization.Roles;
using MFAE.Jobs.Authorization.Users;

namespace MFAE.Jobs.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
