using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Cms.Controllers;

namespace Cms.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : CmsControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
