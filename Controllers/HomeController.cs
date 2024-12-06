using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            var username = HttpContext.Session.GetString("username");
            return View((object)$"Xin chào {username}");
        }

        public IActionResult PersonalTask()
        {
            return RedirectToAction("PersonalTaskList", "PersonalTask");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
