using Microsoft.AspNetCore.Mvc;

namespace ZRdotnetcore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string message)
        {
            ViewData["ConfirmEmailMessage"] = message ?? "";
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
