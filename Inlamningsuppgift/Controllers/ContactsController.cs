using Inlamningsuppgift.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Inlamningsuppgift.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ContactForm form) 
        {
            return View();
        }
    }
}
