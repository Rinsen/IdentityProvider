using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Rinsen.IdentityProviderWeb.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Code401()
        {
            return View();
        }

        public IActionResult Code403()
        {
            return View();
        }

        public IActionResult Code404()
        {
            return View();
        }
    }
}
