using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace Cms.Web.Views
{
    public abstract class CmsRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected CmsRazorPage()
        {
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }
    }
}
