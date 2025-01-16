using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;
using WebNC_BTL_QLCV.Services;

namespace WebNC_BTL_QLCV.Controllers
{
    public class GroupMemberController : Controller
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly INotificationService _notificationService;
     
        public GroupMemberController(IGroupMemberRepository groupMemberRepositor, IUserRepository userRepository, IGroupRepository groupRepository, INotificationService notificationService)
        {
            _groupMemberRepository = groupMemberRepositor;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _notificationService = notificationService;
        }

        public IActionResult GroupMemberList(int groupId)
        {
            int? grId = HttpContext.Session.GetInt32("GroupId");
            var member = _groupMemberRepository.GetGroupMemberByGroupId(grId.Value);
            var group = _groupRepository.GetGroupById(grId.Value);
            int leaderId = group.LeaderID;

            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            //kiểm tra nếu người dùng là trưởng nhóm
            bool isLeader = group.LeaderID == userId;
            ViewBag.IsLeader = isLeader;

            if (userId.HasValue)
            {
                ViewBag.LeaderId = leaderId;
                return View(member);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // thêm thành viên mới
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string username, int groupId)
        {
            int? grId = HttpContext.Session.GetInt32("GroupId");

            if (!grId.HasValue)
            {
                return Json(new { success = false, errorMessage = "GroupId không hợp lệ" });
            }

            var user = _userRepository.GetUserByName(username);

            if (user == null)
            {
                return Json(new { success = false, errorMessage = "Tên thành viên không tồn tại!" });
            }

            bool isMember = _groupMemberRepository.IsUserInGroup(user.UserID, grId.Value);
            if (isMember)
            {
                return Json(new { success = false, errorMessage = "Thành viên đã tồn tại trong nhóm!" });
            }

            DateTime groupEntryDate = DateTime.Now;
            _groupMemberRepository.Add(user.UserID, grId.Value, groupEntryDate);

            var group = _groupRepository.GetGroupById(grId.Value);
            _notificationService.CreateNotification(user.UserID, "Thêm vào nhóm mới", $"Bạn đã được thêm vào nhóm: '{group.GroupName}'", "Thêm vào nhóm");

            return Json(new { success = true });
        }

        // xóa thành viên ra khỏi nhóm
        [HttpPost]
        public IActionResult Delete(int userid, int groupid)
        {
            var group = _groupRepository.GetGroupById(groupid);
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (group != null && group.LeaderID == userId)
            {
                _groupMemberRepository.Delete(userid, groupid);
                return Json(new { success = true });
            }
            return Json(new { success = false, errorMessage = "Không thể xóa thành viên này." });
        }

        // thành viên chủ động rời khỏi nhóm
        [HttpPost]
        public IActionResult LeaveGroup(int groupId, int userId)
        {
            try
            {
                // Kiểm tra nếu người dùng là trưởng nhóm
                var group = _groupRepository.GetGroupById(groupId);
                if (group.LeaderID == userId)
                {
                    return Json(new { success = false, errorMessage = "Bạn không thể rời nhóm khi đang là trưởng nhóm. Vui lòng chuyển quyền trước." });
                }

                // Xóa thành viên khỏi nhóm
                _groupMemberRepository.Delete(userId, groupId);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Gặp lỗi khi rời khỏi nhóm: " + ex.Message });
            }
        }
    }
}
