using Microsoft.AspNetCore.Mvc;

namespace ZRdotnetcore.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/Index
        public IActionResult Index(string message)
        {
            ViewData["ConfirmEmailMessage"] = message ?? "";
            return View();
        }

        //
        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        //
        // GET: /Home/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}
