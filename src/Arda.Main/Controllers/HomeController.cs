using Microsoft.AspNetCore.Mvc;

namespace Arda.Main.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult GetSupport()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
