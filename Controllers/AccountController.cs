using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Repositories;
using WebNC_BTL_QLCV.Models;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace WebNC_BTL_QLCV.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //Đăng ký
        public IActionResult Register()
        {
            ViewData["HideHeaderFooter"] = true;
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var errors = new Dictionary<string, string>();

            // Kiểm tra định dạng email
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            if (!Regex.IsMatch(user.Email, emailPattern))
            {
                errors["Email"] = "Định dạng email không hợp lệ.";
            }

            // Kiểm tra nếu email đã tồn tại
            if (_userRepository.IsEmailExist(user.Email))
            {
                errors["Email"] = "Tài khoản email đã được đăng ký.";
            }

            // Kiểm tra nếu số điện thoại đã tồn tại
            if (_userRepository.IsPhoneNumberExist(user.PhoneNumber))
            {
                errors["PhoneNumber"] = "Số điện thoại đã được đăng ký.";
            }

            // Kiểm tra nếu tên tài khoản đã tồn tại
            if (_userRepository.IsUserNameExist(user.UserName))
            {
                errors["UserName"] = "Tên tài khoản đã được đăng ký.";
            }

            // Nếu có lỗi, trả về thông báo lỗi dưới dạng JSON
            if (errors.Any())
            {
                return Json(new { success = false, errors = errors });
            }

            // Mã hóa mật khẩu người dùng trước khi lưu vào cơ sở dữ liệu
            /*
            var passwordHasher = new PasswordHasher<User>();
            user.PassWord = passwordHasher.HashPassword(user, user.PassWord);
            */

            // Nếu không có lỗi, thêm người dùng vào cơ sở dữ liệu
            user.User_CreationDate = DateTime.Now;
            _userRepository.Add(user);

            // Trả về phản hồi thành công
            return Json(new { success = true, redirectUrl = Url.Action("Login", "Account") });

        }

        //Đăng nhập
        public IActionResult Login()
        {
            ViewData["HideHeaderFooter"] = true;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            string safeUsername = WebUtility.HtmlEncode(username);
            string safePassword = WebUtility.HtmlEncode(password);

            var user = _userRepository.GetUserByName(safeUsername);
            // Tài khoản admin cố định
            string adminUsername = "admin";
            string adminPassword = "1";

            /*
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PassWord, password);  // So sánh mật khẩu đã băm với mật khẩu người dùng nhập
            */

            // Kiểm tra nếu tài khoản là admin và mật khẩu đúng
            if (safeUsername.ToLower() == adminUsername && safePassword == adminPassword)
            {
                HttpContext.Session.SetString("username", safeUsername);
                HttpContext.Session.SetInt32("UserId", 0);
                return Json(new { success = true, redirectUrl = Url.Action("AdminDashboard", "Account") });
            }

            // Kiểm tra tài khoản người dùng thông thường
            if (!_userRepository.IsUserNameExist(safeUsername))
            {
                return Json(new { success = false, message = "Tài khoản không tồn tại!" });
            }

            // Kiểm tra mật khẩu
            if (!_userRepository.IsPasswordCorrect(safeUsername, safePassword))
            {
                return Json(new { success = false, message = "Sai mật khẩu!" });
            }

            // Nếu đăng nhập thành công
            HttpContext.Session.SetInt32("UserId", user.UserID);
            HttpContext.Session.SetString("username", safeUsername);

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }

        public IActionResult AdminDashboard()
        {
            var users = _userRepository.GetAllUsers(); 
             // Thống kê số lượng tài khoản tạo trong tháng và tuần
            var userCountMonth = _userRepository.GetUserCountInCurrentMonth();
            var userCountWeek = _userRepository.GetUserCountInCurrentWeek();

            ViewData["UserCountMonth"] = userCountMonth;
            ViewData["UserCountWeek"] = userCountWeek;
            
            return View(users); 
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
