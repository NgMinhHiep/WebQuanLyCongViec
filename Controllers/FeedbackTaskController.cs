using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Controllers
{
    public class FeedbackTaskController : Controller
    {
        private readonly IFeedbackTaskRepository _feedbackRepo;
        private readonly IGroupRepository _groupRepository;

        public FeedbackTaskController(IFeedbackTaskRepository feedbackRepo, IGroupRepository groupRepository)
        {
            _feedbackRepo = feedbackRepo;
            _groupRepository = groupRepository;
        }

        public IActionResult FeedbackTaskList(int reportFileId)
        {
            ViewBag.ReportFileId = reportFileId;
            var feedbacks = _feedbackRepo.GetFeedbacksByReportFileId(reportFileId);
            HttpContext.Session.SetInt32("reportFileId", reportFileId);

            int? userId = HttpContext.Session.GetInt32("UserId");
            int? grId = HttpContext.Session.GetInt32("GroupId");
            var group = _groupRepository.GetGroupById(grId.Value);
            // Kiểm tra nếu là trưởng nhóm
            bool isLeader = group.LeaderID == userId;
            ViewBag.IsLeader = isLeader;

            return View(feedbacks);
        }

        public IActionResult FeedbackTaskListByParentTask(int reportFileId)
        {
            ViewBag.ReportFileId = reportFileId;
            var feedbacks = _feedbackRepo.GetFeedbacksByReportFileId(reportFileId);

            return View(feedbacks);
        }

        [HttpGet]
        public IActionResult Create(int reportFileId)
        {
            var feedback = new FeedbackTask
            {
                ReportTaskFileID = reportFileId
            };
            return View(feedback);
        }

        [HttpPost]
        public IActionResult Create(FeedbackTask feedback)
        {
            int? ParentTaskId = HttpContext.Session.GetInt32("ParentGroupTaskId");
            if (ModelState.IsValid)
            {
                feedback.FeedbackTime = DateTime.Now;
                _feedbackRepo.AddFeedback(feedback);
                return RedirectToAction("ReportListByParentTask", "ReportTaskFile", new { parentTaskId = ParentTaskId });
            }
            return View(feedback);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            int? ParentTaskId = HttpContext.Session.GetInt32("ParentGroupTaskId");
            _feedbackRepo.DeleteFeedback(id);

            return RedirectToAction("ReportListByParentTask", "ReportTaskFile", new { parentTaskId = ParentTaskId });
        }
    }
}
