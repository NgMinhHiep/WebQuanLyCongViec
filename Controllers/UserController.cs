using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(); // Trả về view rỗng chứa form
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _userRepository.GetUserById(userId.Value);
            if (user == null)
            {
                ModelState.AddModelError("", "Người dùng không tồn tại.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<User>();

            // Xác minh mật khẩu cũ
            var result = passwordHasher.VerifyHashedPassword(user, user.PassWord, model.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return View(model);
            }

            // Hash mật khẩu mới và cập nhật
            user.PassWord = passwordHasher.HashPassword(user, model.NewPassword);
            _userRepository.UpdateUser(user);

            ViewBag.SuccessMessage = "Đổi mật khẩu thành công!";
            return View();
        }

    }
}
