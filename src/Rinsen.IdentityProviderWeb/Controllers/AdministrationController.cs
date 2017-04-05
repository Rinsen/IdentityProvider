using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rinsen.IdentityProviderWeb.Controllers
{
    [Authorize("AdminsOnly")]
    public class AdministrationController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExternalApplications()
        {
            return PartialView();
            //return View();
        }

        public IActionResult Users()
        {
            return PartialView();


            //return View();
        }

    }
}
