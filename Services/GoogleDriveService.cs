using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace WebNC_BTL_QLCV.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly string _folderId = "11fvYeTcCPPY3MOvavdtOOmOIe6X7U-qq"; // Folder trên Google Drive

        public GoogleDriveService()
        {
            var credential = GoogleCredential.FromFile("wwwroot/credentials.json")
                .CreateScoped(DriveService.ScopeConstants.DriveFile);

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveAPI"
            });
        }

        public async Task<(string fileId, double fileSize, string fileType)> UploadFileAsync(Stream fileStream, string fileName, string mimeType)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName,
                Parents = new[] { _folderId }
            };

            var request = _driveService.Files.Create(fileMetadata, fileStream, mimeType);
            request.Fields = "id, size, mimeType";
            var response = await request.UploadAsync();

            if (response.Status == UploadStatus.Completed)
            {
                var uploadedFile = request.ResponseBody;
                double fileSizeMB = uploadedFile.Size.HasValue ? uploadedFile.Size.Value / (1024.0 * 1024.0) : 0;
                return (uploadedFile.Id, fileSizeMB, uploadedFile.MimeType);
            }

            throw new Exception("Tải lên thất bại.");
        }

        public async Task<Stream> DownloadFileAsync(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Position = 0;
            return stream;
        }

        public async Task<bool> DeleteFileFromGoogleDriveAsync(string driveFileId)
        {
            try
            {
                var request = _driveService.Files.Delete(driveFileId);
                await request.ExecuteAsync();  // Xóa file trên Drive
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
