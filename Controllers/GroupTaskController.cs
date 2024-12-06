using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class GroupTaskController : Controller
    {
        private readonly IGroupTaskRepository _GroupTaskRepository;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly IGroupRepository _groupRepository;

        public GroupTaskController(IGroupTaskRepository GroupTaskRepository, ITaskAssignmentRepository taskAssignmentRepository, IGroupRepository groupRepository)
        {
            _GroupTaskRepository = GroupTaskRepository;
            _taskAssignmentRepository = taskAssignmentRepository;
            _groupRepository = groupRepository;
        }

        public IActionResult GroupTaskList(int groupId)
        {

            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? grId = HttpContext.Session.GetInt32("GroupId");
            var task = _GroupTaskRepository.GetGroupTaskByGroupID(grId.Value);
                       
            int? userId = HttpContext.Session.GetInt32("UserId");
            var group = _groupRepository.GetGroupById(grId.Value);

            //kiểm tra nếu người dùng là trưởng nhóm
            bool isLeader = group.LeaderID == userId;
            ViewBag.IsLeader = isLeader;

            if (userId.HasValue)
            {
                return View(task);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // quay về trang danh sách group, xóa session groupid và groupname
        public IActionResult GroupList()
        {
            HttpContext.Session.Remove("GroupId");
            HttpContext.Session.Remove("GroupName");
            return RedirectToAction("GroupList", "Group");

        }       
        
       
        // thêm công việc mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupTask GroupTask)
        {
            int? groupId = HttpContext.Session.GetInt32("GroupId");

            if (!groupId.HasValue)
            {
                return Json(new { success = false, message = "GroupId không hợp lệ." });
            }

            if (ModelState.IsValid)
            {
                if (GroupTask.StartDate > GroupTask.EndDate)
                {
                    return Json(new { success = false, message = "Ngày bắt đầu không được lớn hơn ngày kết thúc." });
                }

                GroupTask.GroupID = groupId.Value;
                _GroupTaskRepository.AddGroupTask(GroupTask);
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        // sửa công việc
        [HttpGet]
        public IActionResult Update(int id)
        {
            var GroupTask = _GroupTaskRepository.GetGroupTaskById(id);
            if (GroupTask == null || GroupTask.GroupID != HttpContext.Session.GetInt32("GroupId"))
            {
                return Unauthorized();
            }
            return View(GroupTask);
        }

        [HttpPost]
        public IActionResult Update(GroupTask groupTask)
        {
            int? groupId = HttpContext.Session.GetInt32("GroupId");
            groupTask.GroupID = groupId.Value;

            if (ModelState.IsValid)
            {
                if (groupTask.StartDate > groupTask.EndDate)
                {
                    ModelState.AddModelError("EndDate", "Ngày bắt đầu không được lớn hơn ngày kết thúc");
                }
                else
                {
                    _GroupTaskRepository.UpdateGroupTask(groupTask);
                    // Trả về kết quả thành công dưới dạng JSON
                    return Json(new { success = true });
                }
            }

            // Xử lý lỗi và trả về JSON chứa thông báo lỗi
            var errorMessage = string.Join("<br />", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, errorMessage });
        }

        // xóa công việc
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var groupTask = _GroupTaskRepository.GetGroupTaskById(id);
            int? groupId = HttpContext.Session.GetInt32("GroupId");

            if (groupTask != null && groupTask.GroupID == groupId)
            {
                _GroupTaskRepository.DeleteGroupTask(id);

                // Trả về JSON để AJAX xử lý
                return Json(new { success = true });
            }

            // Trả về JSON khi không thành công
            return Json(new { success = false, message = "Bạn không có quyền xóa công việc này hoặc công việc không tồn tại." });
        }

        // đến trang phân công thành viên cho công việc

        [HttpGet]
        public IActionResult TaskAssignment(int id)
        {
            var task = _GroupTaskRepository.GetGroupTaskById(id);
            HttpContext.Session.SetInt32("GroupTaskId", task.GroupTaskID);
            return RedirectToAction("AddMember", "TaskAssignment");
        }

        [HttpGet]
        public IActionResult GroupNote(int id)
        {
            var task = _GroupTaskRepository.GetGroupTaskById(id);
            HttpContext.Session.SetInt32("GroupTaskId", task.GroupTaskID);
            HttpContext.Session.SetString("GroupTaskName", task.GroupTaskName);
            return RedirectToAction("GroupNoteList", "GroupNote");
        }

    }
}
