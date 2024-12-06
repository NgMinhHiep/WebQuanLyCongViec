using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupRepository _GroupRepository;
        private readonly IGroupMemberRepository _GroupMemberRepository;

        public GroupController(IGroupRepository GroupRepository, IGroupMemberRepository groupMemberRepository)
        {
            _GroupRepository = GroupRepository;
            _GroupMemberRepository = groupMemberRepository;
        }
        public IActionResult Index()
        {
            var Groups = _GroupRepository.GetAllGroups();
            return View(Groups);
        }

        public IActionResult GroupList()
        {
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                //int userId_int = userId.Value;
                var Groups = _GroupRepository.GetGroupsByUserID(userId.Value);
                var groupUserIsMember = _GroupRepository.GetGroupsUserIsMember(userId.Value);
                ViewBag.GroupsCreatedByUser = Groups;
                ViewBag.GroupsUserIsMember = groupUserIsMember;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult TaskList(int id)
        {
            var group = _GroupRepository.GetGroupById(id);
            HttpContext.Session.SetInt32("GroupId", group.GroupID); // lưu id group vào session
            HttpContext.Session.SetString("GroupName", group.GroupName); // lưu tên group vào session
            return RedirectToAction("GroupTaskList", "GroupTask");
        }

        // thêm nhóm mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Group group)
        {
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để tạo nhóm." });
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                if (userId.HasValue)
                {
                    group.LeaderID = userId.Value;
                    group.GroupFormationDate = DateOnly.FromDateTime(DateTime.Now);
                    _GroupRepository.AddGroup(group);

                    var member = new GroupMember
                    {
                        UserID = userId.Value,
                        GroupID = group.GroupID,
                        GroupEntryDate = DateTime.Now
                    };
                    _GroupMemberRepository.Add(member);

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Không thể xác định người dùng." });
                }
            }

            var errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList();

            return Json(new { success = false, message = string.Join("\n", errors) });
        }

        // sửa thông tin nhóm
        [HttpGet]
        public IActionResult Update(int id)
        {
            var Group = _GroupRepository.GetGroupById(id);
            if (Group == null || Group.LeaderID != HttpContext.Session.GetInt32("UserId"))
            {
                return Unauthorized();
            }
            return View(Group);
        }

        [HttpPost]
        public IActionResult Update(Group group)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                // Kiểm tra quyền của người dùng
                var existingGroup = _GroupRepository.GetGroupById(group.GroupID);
                if (existingGroup == null || existingGroup.LeaderID != userId.Value)
                {
                    return Unauthorized();
                }

                // Cập nhật nhóm
                existingGroup.GroupName = group.GroupName;
                _GroupRepository.UpdateGroup(existingGroup);

                // Trả về JSON để AJAX nhận
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "User không hợp lệ" });
        }

        // xóa nhóm
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var group = _GroupRepository.GetGroupById(id);
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (group != null && group.LeaderID == userId)
            {
                try
                {
                    _GroupRepository.DeleteGroup(id);
                    return Json(new { success = true }); // Trả về JSON cho AJAX
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Lỗi: " + ex.Message });
                }
            }
            return Json(new { success = false, message = "Bạn không có quyền xóa nhóm này." });
        }


        //Test
        public IActionResult Test()
        {
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                //int userId_int = userId.Value;
                var Groups = _GroupRepository.GetGroupsByUserID(userId.Value);
                var groupUserIsMember = _GroupRepository.GetGroupsUserIsMember(userId.Value);
                ViewBag.GroupsCreatedByUser = Groups;
                ViewBag.GroupsUserIsMember = groupUserIsMember;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult CreateTest()
        {
            return PartialView("CreateTest");
        }

        public IActionResult UpdateTest()
        {
            return PartialView("UpdateTest");
        }

        public IActionResult DeleteTest()
        {
            return PartialView("DeleteTest");
        }

        public IActionResult ReadTest()
        {
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                //int userId_int = userId.Value;
                var Groups = _GroupRepository.GetGroupsByUserID(userId.Value);
                var groupUserIsMember = _GroupRepository.GetGroupsUserIsMember(userId.Value);
                ViewBag.GroupsCreatedByUser = Groups;
                ViewBag.GroupsUserIsMember = groupUserIsMember;
                return PartialView("ReadTest", Groups);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
