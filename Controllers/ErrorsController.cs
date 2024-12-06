using Microsoft.AspNetCore.Mvc;

namespace WebNC_BTL_QLCV.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult NoAccess()
        {
            return View();
        }
    }
}
