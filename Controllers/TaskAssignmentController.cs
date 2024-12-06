using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Text.RegularExpressions;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class TaskAssignmentController : Controller
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly IUserRepository _userRepository;

        public TaskAssignmentController(IGroupMemberRepository groupMemberRepository, ITaskAssignmentRepository taskAssignmentRepository, IUserRepository userRepository)
        {
            _groupMemberRepository = groupMemberRepository;
            _taskAssignmentRepository = taskAssignmentRepository;
            _userRepository = userRepository;
        }

        // thêm thành viên phụ trách công việc       
        /*
        [HttpGet]
        public IActionResult Add()
        {
            int? grId = HttpContext.Session.GetInt32("GroupId");
            var member = _groupMemberRepository.GetGroupMemberByGroupId(grId.Value);
            return View(member);

        }

        [HttpPost]
        public IActionResult Add(int id)
        {
            int? grId = HttpContext.Session.GetInt32("GroupId");
            int? taskId = HttpContext.Session.GetInt32("GroupTaskId");

            if (!grId.HasValue)
            {
                return BadRequest("GroupId ko hợp lệ");
            }

            if (grId.HasValue && taskId.HasValue)
            {
                if (ModelState.IsValid)
                {
                    var user = _userRepository.GetUserById(id);
                    //var member = _groupMemberRepository.GetGroupMemberByGroupId(grId.Value);
                    
                    _taskAssignmentRepository.Add(user.UserID, grId.Value, taskId.Value);
                    //return RedirectToAction("GroupTaskList", "GroupTask");
                    return RedirectToAction("GroupTaskList", "GroupTask");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        */
        
        public IActionResult AddMember()
        {
            int? groupId = HttpContext.Session.GetInt32("GroupId");
            int? taskId = HttpContext.Session.GetInt32("GroupTaskId");

            if (!groupId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // lấy danh sách thành viên trong nhóm
            var members = _groupMemberRepository.GetGroupMemberByGroupId(groupId.Value);

            // Tạo ViewModel để chứa thông tin thành viên và trạng thái phân công
            var viewModel = members.Select(m => new MemberViewModel
            {
                UserID = m.UserID,
                UserName = m.User.UserName,
                IsAssigned = _taskAssignmentRepository.IsMemberAssigned(taskId.Value, m.UserID)
            }).ToList();


            ViewBag.TaskId = taskId;

            return View(viewModel);


        }

        [HttpPost]
        public IActionResult Add(int userId)
        {
            int? grId = HttpContext.Session.GetInt32("GroupId");
            int? taskId = HttpContext.Session.GetInt32("GroupTaskId");
            
            if (grId == null || taskId == 0 || userId == 0)
            {
                return BadRequest("Invalid Group or Task ID");
            }

            if (!_taskAssignmentRepository.IsMemberAssigned(taskId.Value, userId))
            {

                _taskAssignmentRepository.Add(userId, grId.Value, taskId.Value);
            }

            return RedirectToAction("AddMember", new {taskId});
        }

        [HttpPost]
        public IActionResult Delete(int taskId, int userId)
        {
            try
            {
                _taskAssignmentRepository.Delete(userId, taskId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                return Json(new { success = false, message = "Xóa thất bại: " + ex.Message });
            }
        }
    }
}
