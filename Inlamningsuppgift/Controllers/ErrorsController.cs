using Microsoft.AspNetCore.Mvc;

namespace Inlamningsuppgift.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
