using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Cms.Controllers
{
    public abstract class CmsControllerBase: AbpController
    {
        protected CmsControllerBase()
        {
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
