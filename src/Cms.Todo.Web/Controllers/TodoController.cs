using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cms.Todo.Web.Controllers
{
    [Area("todo")]
    public class TodoController : AbpController
    {
        public TodoController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
