using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Cms.Controllers;

namespace Cms.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : CmsControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
