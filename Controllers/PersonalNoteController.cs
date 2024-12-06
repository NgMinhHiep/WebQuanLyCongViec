using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class PersonalNoteController : Controller
    {
        private readonly IPersonalNoteRepository _personalNoteRepository;

        public PersonalNoteController(IPersonalNoteRepository personalNoteRepository)
        {
            _personalNoteRepository = personalNoteRepository;
        }
        
        public IActionResult PersonalNoteList(int personalTaskId)
        {
            int? taskId = HttpContext.Session.GetInt32("PersonalTaskId");
            //HttpContext.Session.SetInt32("PersonalTaskId", personalTaskId);
            var note = _personalNoteRepository.GetPersonalNoteByPersonalTaskID(taskId.Value);
            
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }
            
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                return View(note);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public IActionResult TaskList()
        {
            HttpContext.Session.Remove("PersonalTaskId");
            HttpContext.Session.Remove("PersonalTaskName");
            return RedirectToAction("PersonalTaskList", "PersonalTask");
            
        }
        
        // thêm ghi chú mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] PersonalNote personalNote)
        {
            int? taskId = HttpContext.Session.GetInt32("PersonalTaskId");

            if (!taskId.HasValue)
            {
                return Json(new { success = false, message = "TaskId không hợp lệ." });
            }

            personalNote.PersonalTaskID = taskId.Value;
            personalNote.PersonalNote_CreationDate = DateTime.Now;

            try
            {
                _personalNoteRepository.AddPersonalNote(personalNote);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi nếu xảy ra lỗi trong quá trình thêm
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // sửa ghi chú
        [HttpGet]
        public IActionResult Update(int id)
        {
            var personalNote = _personalNoteRepository.GetPersonalNoteById(id);
            if (personalNote == null || personalNote.PersonalTaskID != HttpContext.Session.GetInt32("PersonalTaskId"))
            {
                return Unauthorized();
            }
            return View(personalNote);
        }

        [HttpPost]
        public IActionResult Update(PersonalNote personalNote)
        {
            int? taskId = HttpContext.Session.GetInt32("PersonalTaskId");
            personalNote.PersonalTaskID = taskId.Value;
            
            if (ModelState.IsValid)
            {
                    _personalNoteRepository.UpdatePersonalNote(personalNote);
                    return RedirectToAction("PersonalNoteList");
            }

            return View(personalNote);
        }

        // xóa ghi chú
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var personalNote = _personalNoteRepository.GetPersonalNoteById(id);
            int? taskId = HttpContext.Session.GetInt32("PersonalTaskId");

            if (personalNote != null && personalNote.PersonalTaskID == taskId)
            {
                _personalNoteRepository.DeletePersonalNote(id);
                // Trả về JSON để AJAX có thể xử lý
                return Json(new { success = true });
            }

            // Trả về lỗi nếu không tìm thấy ghi chú hoặc người dùng không hợp lệ
            return Json(new { success = false });
        }
    }
}
