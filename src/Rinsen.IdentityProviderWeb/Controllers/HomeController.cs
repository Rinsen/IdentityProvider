using Microsoft.AspNetCore.Mvc;
using System;

namespace Rinsen.IdentityProviderWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ThrowError()
        {
            throw new Exception();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
