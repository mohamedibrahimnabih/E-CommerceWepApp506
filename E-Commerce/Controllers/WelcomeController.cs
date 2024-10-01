using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            string name = "Mohamed Ibrahim";

            return View(model: name);
        }
    }
}
