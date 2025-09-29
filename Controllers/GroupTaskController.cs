using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;
using WebNC_BTL_QLCV.Repositories.IRepository;
using WebNC_BTL_QLCV.Services;

namespace WebNC_BTL_QLCV.Controllers
{
    public class GroupTaskController : Controller
    {
        private readonly IGroupTaskRepository _GroupTaskRepository;

        private readonly IGroupRepository _groupRepository;

        private readonly IParentGroupTaskRepository _parentGroupTaskRepository;

        private readonly IGroupMemberRepository _groupMemberRepository;
        
        private readonly INotificationService _notificationService;
        
        private readonly INotificationRepository _notificationRepository;



        public GroupTaskController(IGroupTaskRepository GroupTaskRepository, IGroupRepository groupRepository, IParentGroupTaskRepository parentGroupTaskRepository, IGroupMemberRepository groupMemberRepository
            , INotificationService notificationService, INotificationRepository notificationRepository)
        {
            _GroupTaskRepository = GroupTaskRepository;
            _groupRepository = groupRepository;
            _parentGroupTaskRepository = parentGroupTaskRepository;
            _groupMemberRepository = groupMemberRepository;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
        }

        public IActionResult GroupTaskList(int parenttaskid)
        {
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? parentTaskId = HttpContext.Session.GetInt32("ParentGroupTaskId");
            int? userId = HttpContext.Session.GetInt32("UserId");
            int? grId = HttpContext.Session.GetInt32("GroupId");

            var group = _groupRepository.GetGroupById(grId.Value);

            // Lấy danh sách thành viên trong nhóm
            var groupMembers = _groupMemberRepository.GetGroupMemberByGroupId(grId.Value);
            ViewBag.GroupMembers = groupMembers;

            // Kiểm tra nếu là trưởng nhóm
            bool isLeader = group.LeaderID == userId;
            ViewBag.IsLeader = isLeader;

            // Lấy danh sách công việc theo ParentTaskID
            var tasks = _GroupTaskRepository.GetGroupTasksByParentGroupTaskID(parentTaskId.Value)
                .OrderBy(task =>
                {
                    if (task.PriorityLevel != null && task.PriorityLevel.StartsWith("Số"))
                    {
                        var parts = task.PriorityLevel.Split(' ');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int level))
                        {
                            return level; // sắp theo số cấp
                        }
                    }
                    return int.MaxValue; // nếu không phải kiểu "Cấp x" thì để xuống cuối
                })
                .ToList();

            // Đếm thống kê công việc
            ViewBag.TotalTasks = tasks.Count;
            ViewBag.CompletedTasks = tasks.Count(t => t.GroupTaskStatus == "Đã hoàn thành");
            ViewBag.InProgressTasks = tasks.Count(t => t.GroupTaskStatus == "Đang thực hiện");
            ViewBag.NotStartedTasks = tasks.Count(t => t.GroupTaskStatus == "Chưa hoàn thành");

            // Đếm thống kê công việc cá nhân
            ViewBag.PersonalTotalTasks = tasks.Count(t => t.UserID == userId);
            ViewBag.PersonalCompletedTasks = tasks.Count(t => t.GroupTaskStatus == "Đã hoàn thành" && t.UserID == userId);
            ViewBag.PersonalInProgressTasks = tasks.Count(t => t.GroupTaskStatus == "Đang thực hiện" && t.UserID == userId);
            ViewBag.PersonalNotStartedTasks = tasks.Count(t => t.GroupTaskStatus == "Chưa hoàn thành" && t.UserID == userId);

            if (userId.HasValue)
            {
                return View(tasks);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        // quay về trang danh sách group, xóa session groupid và groupname
        public IActionResult ParentGroupTaskList()
        {
            HttpContext.Session.Remove("ParentGroupTaskId");
            return RedirectToAction("ParentGroupTaskList", "ParentGroupTask");

        }       
        
       
        // thêm công việc mới
        [HttpGet]
        public IActionResult Create()
        {          
            int? grId = HttpContext.Session.GetInt32("GroupId");
            int? parentTaskId = HttpContext.Session.GetInt32("ParentGroupTaskId");

            if (!grId.HasValue)
            {
                return RedirectToAction("GroupList", "Group");
            }

            // Lấy danh sách thành viên trong nhóm (đã có sẵn của bạn)
            var members = _groupMemberRepository.GetGroupMemberByGroupId(grId.Value)
                .Select(m => new SelectListItem
                {
                    Value = m.UserID.ToString(),
                    Text = m.User != null ? m.User.FullName : m.UserID.ToString()
                }).ToList();

            ViewBag.GroupMembers = members;

            // ==== BẮT ĐẦU: LỌC DANH SÁCH ĐỘ ƯU TIÊN ====

            List<string> fixedPriorities = new List<string> { "Không" }; // luôn được hiển thị
            List<string> uniquePriorities = new List<string> { "Số 1", "Số 2", "Số 3",  "Số 4", "Số 5", "Số 6", "Số 7", "Số 8", "Số 9", "Số 10" };

            // Lấy danh sách công việc hiện có trong nhóm hiện tại
            var existingTasks = _GroupTaskRepository.GetGroupTasksByParentGroupTaskID(parentTaskId.Value); // hoặc theo parenttaskid nếu bạn dùng kiểu đó

            var usedUniquePriorities = existingTasks
                .Where(t => t.PriorityLevel != "Không")
                .Select(t => t.PriorityLevel)
                .Distinct()
                .ToList();

            var remainingUnique = uniquePriorities.Except(usedUniquePriorities).ToList();

            var allAvailablePriorities = fixedPriorities.Concat(remainingUnique).ToList();

            ViewBag.AvailablePriorities = allAvailablePriorities;

            // ==== KẾT THÚC ====

            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupTask task)
        {
            int? groupId = HttpContext.Session.GetInt32("GroupId");
            int? parentTaskId = HttpContext.Session.GetInt32("ParentGroupTaskId");
            if (ModelState.IsValid)
            {
                task.ParentGroupTaskID = parentTaskId.Value;
                _GroupTaskRepository.AddGroupTask(task);
                return Json(new { success = true });
            }

            // Nếu lỗi validation, nạp lại danh sách
            var members = _groupMemberRepository.GetGroupMemberByGroupId(groupId.Value);
            ViewBag.Members = new SelectList(members, "UserID", "User.FullName");
            return View(task);
        }


        // sửa công việc
        [HttpGet]
        public IActionResult Update(int id)
        {
            var GroupTask = _GroupTaskRepository.GetGroupTaskById(id);
            int? parentId = HttpContext.Session.GetInt32("ParentGroupTaskId");
            int? grId = HttpContext.Session.GetInt32("GroupId");

            if (GroupTask == null || GroupTask.ParentGroupTaskID != parentId)
            {
                return Unauthorized();
            }

            // Lấy danh sách thành viên trong nhóm
            var members = _groupMemberRepository.GetGroupMemberByGroupId(grId.Value)
                .Select(m => new SelectListItem
                {
                    Value = m.UserID.ToString(),
                    Text = m.User != null ? m.User.FullName : m.UserID.ToString()
                }).ToList();

            ViewBag.GroupMembers = members;

            // Danh sách độ ưu tiên gốc
            List<string> fixedPriorities = new List<string> { "Không" }; // luôn hiển thị
            List<string> uniquePriorities = new List<string> { "Số 1", "Số 2", "Số 3", "Số 4", "Số 5", "Số 6", "Số 7", "Số 8", "Số 9", "Số 10" };

            // Lấy danh sách công việc hiện tại trong nhóm (trừ công việc đang sửa)
            var existingTasks = _GroupTaskRepository.GetGroupTasksByParentGroupTaskID(parentId.Value)
                .Where(t => t.GroupTaskID != id) // loại trừ công việc đang sửa
                .ToList();

            // Lấy những độ ưu tiên đã dùng (trừ "Không")
            var usedUniquePriorities = existingTasks
                .Where(t => t.PriorityLevel != "Không")
                .Select(t => t.PriorityLevel)
                .Distinct()
                .ToList();

            // Trường hợp đặc biệt: nếu độ ưu tiên của task đang sửa đã bị dùng rồi, vẫn phải thêm lại
            if (!usedUniquePriorities.Contains(GroupTask.PriorityLevel)
                && GroupTask.PriorityLevel != "Không"
                && uniquePriorities.Contains(GroupTask.PriorityLevel))
            {
                usedUniquePriorities.Add(GroupTask.PriorityLevel);
            }

            var remainingUnique = uniquePriorities.Except(usedUniquePriorities).ToList();

            // Danh sách cuối cùng: "Không" + còn lại + giá trị đang chọn (nếu bị trùng)
            var finalPriorities = fixedPriorities.Concat(remainingUnique).ToList();

            if (!finalPriorities.Contains(GroupTask.PriorityLevel))
            {
                finalPriorities = finalPriorities.Append(GroupTask.PriorityLevel).ToList();
            }

            ViewBag.AvailablePriorities = finalPriorities;
            
            /*
            // test gửi thông báo khi thay đổi công việc
            var tasks = _GroupTaskRepository.GetGroupTasksByParentGroupTaskID(parentId.Value);
            
            foreach(var task in tasks)
            {
                var groupName = task.ParentGroupTask.Group.GroupName;
                var parentTaskName = task.ParentGroupTask?.ParentGroupTaskName;

                _notificationService.CreateNotification(GroupTask.UserID, "Cập nhật công việc", $"Công việc \"{task.GroupTaskName}\" (thuộc công việc cha \"{parentTaskName}\" của nhóm \"{groupName}\") đã được cập nhật thông tin bởi trưởng nhóm", "Cập nhật công việc");
            }
            */
            
            return View(GroupTask);
        }


        [HttpPost]
        public IActionResult Update(GroupTask groupTask)
        {
            int? ParentGroupTaskId = HttpContext.Session.GetInt32("ParentGroupTaskId");
            groupTask.ParentGroupTaskID = ParentGroupTaskId.Value;

            if (ModelState.IsValid)
            {
                if (groupTask.StartDate > groupTask.EndDate)
                {
                    ModelState.AddModelError("EndDate", "Ngày bắt đầu không được lớn hơn ngày kết thúc");
                    // Nạp lại danh sách thành viên
                    int? groupId = HttpContext.Session.GetInt32("GroupId");
                    var members = _groupMemberRepository.GetGroupMemberByGroupId(groupId.Value)
                                    .Select(m => new SelectListItem
                                    {
                                        Value = m.UserID.ToString(),
                                        Text = m.User != null ? m.User.FullName : m.UserID.ToString()
                                    }).ToList();
                    ViewBag.GroupMembers = members;
                    return View(groupTask);
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
            int? ParentGroupTaskId = HttpContext.Session.GetInt32("ParentGroupTaskId");

            if (groupTask != null && groupTask.ParentGroupTaskID == ParentGroupTaskId)
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

        // Hiện nút gia hạn: form chọn ngày mới
        [HttpGet]
        public IActionResult ExtendDeadline(int id)
        {
            var task = _GroupTaskRepository.GetGroupTaskById(id);
            if (task == null) return NotFound();

            var vm = new ExtendDeadlineViewModel
            {
                GroupTaskID = id,
                CurrentEndDate = task.EndDate
            };
            return View(vm);
        }

        // Xử lý POST gia hạn
        [HttpPost]
        public IActionResult ExtendDeadline(ExtendDeadlineViewModel vm)
        {
            if (vm.NewEndDate <= vm.CurrentEndDate)
            {
                ModelState.AddModelError("NewEndDate", "Ngày mới phải sau ngày cũ.");
                return View(vm);
            }

            _GroupTaskRepository.ExtendDeadline(vm.GroupTaskID, vm.NewEndDate);
            return RedirectToAction("GroupTaskList", new { parenttaskid = /* lấy parentId từ repo */ _GroupTaskRepository.GetGroupTaskById(vm.GroupTaskID).ParentGroupTaskID });
        }

        public IActionResult MemberTaskStats()
        {
            int? groupId = HttpContext.Session.GetInt32("GroupId");
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (groupId == null || userId == null)
                return RedirectToAction("Login", "Account");

            var group = _groupRepository.GetGroupById(groupId.Value);

            if (group.LeaderID != userId)
                return Unauthorized();

            var stats = _GroupTaskRepository.GetMemberTaskStatsByGroupId(groupId.Value);
            return View(stats);
        }

    }
}
