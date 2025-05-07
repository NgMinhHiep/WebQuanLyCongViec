using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Controllers
{
    public class ParentGroupTaskController : Controller
    {
        private readonly IParentGroupTaskRepository _ParentGroupTaskRepository;

        private readonly IGroupRepository _groupRepository;


        public ParentGroupTaskController(IParentGroupTaskRepository ParentGroupTaskRepository, IGroupRepository groupRepository)
        {
            _ParentGroupTaskRepository = ParentGroupTaskRepository;
            _groupRepository = groupRepository;
        }

        public IActionResult ParentGroupTaskList(int groupId)
        {

            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "Account");
            }

            int? grId = HttpContext.Session.GetInt32("GroupId");
            var task = _ParentGroupTaskRepository.GetParentGroupTasksByGroupID(grId.Value);

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

        // sang trang group task
        public IActionResult GroupTaskList(int parenttaskid)
        {
            var task = _ParentGroupTaskRepository.GetParentGroupTaskById(parenttaskid);
            HttpContext.Session.SetInt32("ParentGroupTaskId", task.ParentGroupTaskID); // lưu id parent task vào session
            HttpContext.Session.SetString("ParentGroupTaskName", task.ParentGroupTaskName);
            return RedirectToAction("GroupTaskList", "GroupTask");
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
            int? grId = HttpContext.Session.GetInt32("GroupId");

            if (!grId.HasValue)
            {
                return RedirectToAction("GroupList", "Group");
            }
        
            // ==== BẮT ĐẦU: LỌC DANH SÁCH ĐỘ ƯU TIÊN ====

            List<string> fixedPriorities = new List<string> { "Không" }; // luôn được hiển thị
            List<string> uniquePriorities = new List<string> { "Số 1", "Số 2", "Số 3", "Số 4", "Số 5", "Số 6", "Số 7", "Số 8", "Số 9", "Số 10" };

            // Lấy danh sách công việc hiện có trong nhóm hiện tại
            var existingTasks = _ParentGroupTaskRepository.GetParentGroupTasksByGroupID(grId.Value); 

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
        public IActionResult Create(ParentGroupTask ParentGroupTask)
        {
            int? groupId = HttpContext.Session.GetInt32("GroupId");

            if (!groupId.HasValue)
            {
                return Json(new { success = false, message = "GroupId không hợp lệ." });
            }

            if (ModelState.IsValid)
            {

                ParentGroupTask.GroupID = groupId.Value;
                _ParentGroupTaskRepository.AddParentGroupTask(ParentGroupTask);
                return Json(new { success = true });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                // Log errors hoặc debug:
                return Json(new { success = false, errors });
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        // sửa công việc
        [HttpGet]
        public IActionResult Update(int id)
        {
            int? grId = HttpContext.Session.GetInt32("GroupId");
            var ParentGroupTask = _ParentGroupTaskRepository.GetParentGroupTaskById(id);
            if (ParentGroupTask == null || ParentGroupTask.GroupID != HttpContext.Session.GetInt32("GroupId"))
            {
                return Unauthorized();
            }

            // Danh sách độ ưu tiên gốc
            List<string> fixedPriorities = new List<string> { "Không" }; // luôn hiển thị
            List<string> uniquePriorities = new List<string> { "Số 1", "Số 2", "Số 3", "Số 4", "Số 5", "Số 6", "Số 7", "Số 8", "Số 9", "Số 10" };

            // Lấy danh sách công việc hiện tại trong nhóm (trừ công việc đang sửa)
            var existingTasks = _ParentGroupTaskRepository.GetParentGroupTasksByGroupID(grId.Value)
                .Where(t => t.ParentGroupTaskID != id) // loại trừ công việc đang sửa
                .ToList();

            // Lấy những độ ưu tiên đã dùng (trừ "Không")
            var usedUniquePriorities = existingTasks
                .Where(t => t.PriorityLevel != "Không")
                .Select(t => t.PriorityLevel)
                .Distinct()
                .ToList();

            // Trường hợp đặc biệt: nếu độ ưu tiên của task đang sửa đã bị dùng rồi, vẫn phải thêm lại
            if (!usedUniquePriorities.Contains(ParentGroupTask.PriorityLevel)
                && ParentGroupTask.PriorityLevel != "Không"
                && uniquePriorities.Contains(ParentGroupTask.PriorityLevel))
            {
                usedUniquePriorities.Add(ParentGroupTask.PriorityLevel);
            }

            var remainingUnique = uniquePriorities.Except(usedUniquePriorities).ToList();

            // Danh sách cuối cùng: "Không" + còn lại + giá trị đang chọn (nếu bị trùng)
            var finalPriorities = fixedPriorities.Concat(remainingUnique).ToList();

            if (!finalPriorities.Contains(ParentGroupTask.PriorityLevel))
            {
                finalPriorities = finalPriorities.Append(ParentGroupTask.PriorityLevel).ToList();
            }

            ViewBag.AvailablePriorities = finalPriorities;

            return View(ParentGroupTask);
        }

        [HttpPost]
        public IActionResult Update(ParentGroupTask ParentGroupTask)
        {
            int? groupId = HttpContext.Session.GetInt32("GroupId");
            ParentGroupTask.GroupID = groupId.Value;

            if (ModelState.IsValid)
            {
                    _ParentGroupTaskRepository.UpdateParentGroupTask(ParentGroupTask);
                    // Trả về kết quả thành công dưới dạng JSON
                    return Json(new { success = true });
            }

            // Xử lý lỗi và trả về JSON chứa thông báo lỗi
            var errorMessage = string.Join("<br />", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, errorMessage });
        }

        // xóa công việc
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var ParentGroupTask = _ParentGroupTaskRepository.GetParentGroupTaskById(id);
            int? groupId = HttpContext.Session.GetInt32("GroupId");

            if (ParentGroupTask != null && ParentGroupTask.GroupID == groupId)
            {
                _ParentGroupTaskRepository.DeleteParentGroupTask(id);

                // Trả về JSON để AJAX xử lý
                return Json(new { success = true });
            }

            // Trả về JSON khi không thành công
            return Json(new { success = false, message = "Bạn không có quyền xóa công việc này hoặc công việc không tồn tại." });
        }
    }
}
