using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityClientWeb.Controllers
{
    
    public class HomeController : Controller
    {
        [AllowAnonymous]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(ActiveAuthenticationSchemes = "Rinsen")]
        // GET: /<controller>/
        public IActionResult Index2()
        {
            return View();
        }
    }
}
