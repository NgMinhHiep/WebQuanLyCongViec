using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class Personal_InformationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public Personal_InformationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Personal_Infor_List()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var user_info = _userRepository.GetUserById(userId.Value);
                return View(user_info);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // sửa thông tin cá nhân
        [HttpGet]
        public IActionResult Update(int id)
        {
            var personalInfo = _userRepository.GetUserById(id);
            if (personalInfo == null || personalInfo.UserID != HttpContext.Session.GetInt32("UserId"))
            {
                return Unauthorized();
            }
            return View(personalInfo);
        }

        [HttpPost]
        public IActionResult Update(User user)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //user.UserID = userId.Value;
                
                // Cập nhật công việc
                _userRepository.Update(user);
                // Trả về kết quả thành công
                return Json(new { success = true });               

            // Nếu có lỗi trong model, trả về thông báo lỗi
            var errorMessage = string.Join("<br />", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, errorMessage });
        }
    }
}
