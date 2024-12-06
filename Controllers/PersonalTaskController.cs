using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class PersonalTaskController : Controller
    {
        private readonly IPersonalTaskRepository _personalTaskRepository;

        public PersonalTaskController(IPersonalTaskRepository personalTaskRepository)
        {
            _personalTaskRepository = personalTaskRepository;
        }
        public IActionResult Index()
        {
            var personalTasks = _personalTaskRepository.GetAllPersonalTasks();
            return View(personalTasks);
        }

        public IActionResult PersonalTaskList()
        {
            if(!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if(userId.HasValue)
            {
                int userId_int = userId.Value;
                var personalTasks = _personalTaskRepository.GetPersonalTasksByUserID(userId_int);
                return View(personalTasks);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            /*
            if (int.TryParse(userId, out int intUserId))
            {
                var personalTasks = _personalTaskRepository.GetPersonalTasksByUserID(intUserId);
                return View(personalTasks);
            }
            else
            {
                return View();
            }
            */
            
        }

        // thêm công việc mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PersonalTask personalTask)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Người dùng chưa đăng nhập." });
            }

            personalTask.UserID = userId.Value;

            if (ModelState.IsValid)
            {
                if (personalTask.StartDate > personalTask.EndDate)
                {
                    return Json(new { success = false, message = "Ngày bắt đầu không được lớn hơn ngày kết thúc." });
                }
                _personalTaskRepository.AddPersonalTask(personalTask);
                return Json(new { success = true, message = "Thêm công việc thành công." });
            }

            // Lấy lỗi từ ModelState và gửi về client
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }

        // sửa công việc
        [HttpGet]
        public IActionResult Update(int id)
        {
            var personalTask = _personalTaskRepository.GetPersonalTaskById(id);
            if (personalTask == null ||  personalTask.UserID != HttpContext.Session.GetInt32("UserId"))
            {
                return Unauthorized();
            }
            return View(personalTask);
        }
        /*
        [HttpPost]
        public IActionResult Update(PersonalTask personalTask)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            personalTask.UserID = userId.Value;
            if (ModelState.IsValid)
            {
                if (personalTask.StartDate > personalTask.EndDate)
                {
                    ModelState.AddModelError("EndDate", "Ngày bắt đầu không được lớn hơn ngày kết thúc");
                    //return View(personalTask);
                }
                else
                {
                    _personalTaskRepository.UpdatePersonalTask(personalTask);
                    return RedirectToAction("PersonalTaskList");

                }
            }

            return View(personalTask);
        }
        */

        [HttpPost]
        public IActionResult Update(PersonalTask personalTask)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            personalTask.UserID = userId.Value;

            // Kiểm tra tính hợp lệ của dữ liệu
            if (ModelState.IsValid)
            {
                if (personalTask.StartDate > personalTask.EndDate)
                {
                    ModelState.AddModelError("EndDate", "Ngày bắt đầu không được lớn hơn ngày kết thúc");
                }
                else
                {
                    // Cập nhật công việc
                    _personalTaskRepository.UpdatePersonalTask(personalTask);
                    // Trả về kết quả thành công
                    return Json(new { success = true });
                }
            }

            // Nếu có lỗi trong model, trả về thông báo lỗi
            var errorMessage = string.Join("<br />", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, errorMessage });
        }

        // xóa công việc      
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var personalTask = _personalTaskRepository.GetPersonalTaskById(id);
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (personalTask != null && personalTask.UserID == userId)
            {
                _personalTaskRepository.DeletePersonalTask(id);
                return Json(new { success = true, message = "Xóa công việc thành công." }); // Trả về JSON
            }
            return Json(new { success = false, message = "Bạn không có quyền xóa công việc này hoặc công việc không tồn tại." });
        }

        public IActionResult NoteList(int id)
        {
            var task = _personalTaskRepository.GetPersonalTaskById(id);
            HttpContext.Session.SetInt32("PersonalTaskId", task.PersonalTaskID); // lưu id công việc
            HttpContext.Session.SetString("PersonalTaskName", task.PersonalTaskName);
            return RedirectToAction("PersonalNoteList", "PersonalNote");
        }
        
        public IActionResult Home()
        {
            return RedirectToAction("Index", "Home");
        }        
    }
}
