using Abp.Authorization;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;

namespace Emarketing.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
