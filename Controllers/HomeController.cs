using Microsoft.AspNetCore.Mvc;

namespace ZRdotnetcore.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/Index
        [HttpGet]
        public IActionResult Index(string message)
        {
            ViewData["ConfirmEmailMessage"] = message ?? "";
            return View();
        }

        //
        // GET: /Home/About
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        //
        // GET: /Home/Error
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
