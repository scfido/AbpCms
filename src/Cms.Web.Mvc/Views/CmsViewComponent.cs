using Abp.AspNetCore.Mvc.ViewComponents;

namespace Cms.Web.Views
{
    public abstract class CmsViewComponent : AbpViewComponent
    {
        protected CmsViewComponent()
        {
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }
    }
}
