using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Emarketing.Controllers
{
    public abstract class EmarketingControllerBase: AbpController
    {
        protected EmarketingControllerBase()
        {
            LocalizationSourceName = EmarketingConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
