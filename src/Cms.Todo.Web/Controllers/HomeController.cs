using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cms.Todo.Web.Controllers
{
    [Area("todo")]
    public class HomeController : AbpController
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
