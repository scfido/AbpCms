using Abp.Authorization;
using Cms.Authorization.Roles;
using Cms.Authorization.Users;

namespace Cms.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
