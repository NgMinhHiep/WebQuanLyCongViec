using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;
using WebNC_BTL_QLCV.Repositories.IRepository;
using WebNC_BTL_QLCV.Services;

namespace WebNC_BTL_QLCV.Controllers
{
    public class ReportTaskFileController : Controller
    {
        private readonly IReportTaskFileRepository _fileRepo;
        private readonly GoogleDriveService _driveService;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupTaskRepository _groupTaskRepository;

        public ReportTaskFileController(IReportTaskFileRepository fileRepo, GoogleDriveService driveService, IGroupRepository groupRepository, IGroupTaskRepository groupTaskRepository)
        {
            _fileRepo = fileRepo;
            _driveService = driveService;
            _groupRepository = groupRepository;
            _groupTaskRepository = groupTaskRepository;
        }


        // Action hiển thị danh sách file theo TaskId 
        public async Task<IActionResult> ReportTaskFileList(int taskid)
        {
            // Lấy danh sách file của công việc nhóm có TaskID = taskid
            var files = await _fileRepo.GetAllFilesAsync(taskid);
            HttpContext.Session.SetInt32("groupTaskId",taskid);

            int? userId = HttpContext.Session.GetInt32("UserId");
            var grouptask = _groupTaskRepository.GetGroupTaskById(taskid);
            // Kiểm tra nếu là thành viên phụ trách
            bool isMemberAsign = grouptask.UserID == userId;
            ViewBag.IsMemberAsign = isMemberAsign;



            // Đưa taskid vào ViewBag để sử dụng trong view 
            ViewBag.TaskId = taskid;
            return View(files);
        }

        // hiển thị danh sách toàn bộ báo cáo của công việc cha
        public IActionResult ReportListByParentTask(int parentTaskId)
        {
            var reports = _fileRepo.GetReportFilesByParentTaskId(parentTaskId);
            return View(reports);
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(int taskid, IFormFile file, string description)
        {
            // Lấy tên người gửi từ session
            string senderName = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(senderName))
                return BadRequest("Tên người gửi không hợp lệ.");

            DateTime fileUploadDate = DateTime.Now;

            using (var stream = file.OpenReadStream())
            {

                var (driveFileId, fileSize, fileType) = await _driveService.UploadFileAsync(stream, file.FileName, file.ContentType);
                if (!string.IsNullOrEmpty(driveFileId))
                {
                    var taskFile = new ReportTaskFile
                    {
                        GroupTaskID = taskid,
                        SenderName = senderName,
                        ReportFileName = file.FileName,
                        GoogleDriveFileId = driveFileId,
                        FileSize = fileSize,
                        FileType = fileType,
                        UploadedTime = fileUploadDate,
                        ReportDescription = description
                    };
                    await _fileRepo.AddFileAsync(taskFile);
                }
            }

            return RedirectToAction("ReportTaskFileList", new { taskid });
        }

        public async Task<IActionResult> DownloadFile(int fileId)
        {
            var file = await _fileRepo.GetFileByIdAsync(fileId);
            var stream = await _driveService.DownloadFileAsync(file.GoogleDriveFileId);
            return File(stream, "application/octet-stream", file.ReportFileName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile(int fileId, int taskId)
        {
            // 1. Lấy thông tin file từ DB
            var file = await _fileRepo.GetFileByIdAsync(fileId);
            if (file == null)
            {
                return NotFound("Không tìm thấy file trong cơ sở dữ liệu.");
            }

            // 2. Xóa file trên Google Drive
            bool isDriveDeleted = await _driveService.DeleteFileFromGoogleDriveAsync(file.GoogleDriveFileId);

            // 3. Xóa file trong DB
            if (isDriveDeleted)
            {
                await _fileRepo.DeleteFileAsync(fileId);
            }
            else
            {
                await _fileRepo.DeleteFileAsync(fileId);
            }

            // 4. Quay lại trang danh sách file của công việc nhóm
            return RedirectToAction("ReportTaskFileList", new { taskid = taskId });
        }
    }
}
