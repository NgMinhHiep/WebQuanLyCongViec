using Microsoft.AspNetCore.Mvc;
using System.Formats.Tar;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories.IRepository;
using WebNC_BTL_QLCV.Services;

namespace WebNC_BTL_QLCV.Controllers
{
    public class GroupTaskFileController : Controller
    {
        private readonly IGroupTaskFileRepository _fileRepo;
        private readonly GoogleDriveService _driveService;

        public GroupTaskFileController(IGroupTaskFileRepository fileRepo, GoogleDriveService driveService)
        {
            _fileRepo = fileRepo;
            _driveService = driveService;
        }


        // Action hiển thị danh sách file theo TaskId 
        public async Task<IActionResult> GroupTaskFileList(int taskid)
        {
            // Lấy danh sách file của công việc nhóm có TaskID = taskid
            var files = await _fileRepo.GetAllFilesAsync(taskid);

            // Đưa taskid vào ViewBag để sử dụng trong view 
            ViewBag.TaskId = taskid;
            return View(files);
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(int taskid, IFormFile file)
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
                    var taskFile = new GroupTaskFile
                    {
                        GroupTaskID = taskid,
                        SenderName = senderName,
                        FileName = file.FileName,
                        GoogleDriveFileId = driveFileId,
                        FileSize = fileSize,
                        FileType = fileType,
                        UploadedTime = fileUploadDate
                    };
                    await _fileRepo.AddFileAsync(taskFile);
                }
            }

            return RedirectToAction("GroupTaskFileList", new { taskid });
        }

        public async Task<IActionResult> DownloadFile(int fileId)
        {
            var file = await _fileRepo.GetFileByIdAsync(fileId);
            var stream = await _driveService.DownloadFileAsync(file.GoogleDriveFileId);
            return File(stream, "application/octet-stream", file.FileName);
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
            return RedirectToAction("GroupTaskFileList", new { taskid = taskId });
        }
    }
}
