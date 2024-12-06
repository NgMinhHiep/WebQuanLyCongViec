using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class GroupNoteController : Controller
    {
        private readonly IGroupNoteRepository _GroupNoteRepository;

        public GroupNoteController(IGroupNoteRepository GroupNoteRepository)
        {
            _GroupNoteRepository = GroupNoteRepository;
        }

        public IActionResult GroupNoteList(int GroupTaskId)
        {
            int? taskId = HttpContext.Session.GetInt32("GroupTaskId");
            var note = _GroupNoteRepository.GetGroupNoteByGroupTaskID(taskId.Value);

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
            HttpContext.Session.Remove("GroupTaskId");
            HttpContext.Session.Remove("GroupTaskName");
            return RedirectToAction("GroupTaskList", "GroupTask");

        }

        // thêm ghi chú mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupNote groupNote)
        {
            int? taskId = HttpContext.Session.GetInt32("GroupTaskId");

            if (!taskId.HasValue)
            {
                return Json(new { success = false, errorMessage = "TaskId không hợp lệ." });
            }

            groupNote.GroupTaskID = taskId.Value;
            groupNote.GroupNote_CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _GroupNoteRepository.AddGroupNote(groupNote);
                return Json(new { success = true });
            }

            var errorMessage = string.Join("<br />", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, errorMessage });
        }

        // sửa ghi chú
        [HttpGet]
        public IActionResult Update(int id)
        {
            var GroupNote = _GroupNoteRepository.GetGroupNoteById(id);
            if (GroupNote == null || GroupNote.GroupTaskID != HttpContext.Session.GetInt32("GroupTaskId"))
            {
                return Unauthorized();
            }
            return View(GroupNote);
        }

        [HttpPost]
        public IActionResult Update(GroupNote GroupNote)
        {
            int? taskId = HttpContext.Session.GetInt32("GroupTaskId");
            GroupNote.GroupTaskID = taskId.Value;

            if (ModelState.IsValid)
            {
                _GroupNoteRepository.UpdateGroupNote(GroupNote);
                return RedirectToAction("GroupNoteList");
            }

            return View(GroupNote);
        }

        // xóa ghi chú
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var groupNote = _GroupNoteRepository.GetGroupNoteById(id);
            int? taskId = HttpContext.Session.GetInt32("GroupTaskId");

            if (groupNote != null && groupNote.GroupTaskID == taskId)
            {
                _GroupNoteRepository.DeleteGroupNote(id);
                return Json(new { success = true });
            }
            return Json(new { success = false, errorMessage = "Không thể xóa ghi chú này." });
        }
    }
}
