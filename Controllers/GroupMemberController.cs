using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class GroupMemberController : Controller
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
     
        public GroupMemberController(IGroupMemberRepository groupMemberRepositor, IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _groupMemberRepository = groupMemberRepositor;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
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
    }
}
