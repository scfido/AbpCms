using Microsoft.AspNetCore.Antiforgery;
using Cms.Controllers;

namespace Cms.Web.Host.Controllers
{
    public class AntiForgeryController : CmsControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
