using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories.IRepository
{
    public interface IReportTaskFileRepository
    {
        Task<List<ReportTaskFile>> GetAllFilesAsync(int taskId); // Lấy file theo TaskId
        IEnumerable<ReportTaskFile> GetReportFilesByParentTaskId(int parentTaskId); // lấy ds file theo id công việc cha
        Task<ReportTaskFile> GetFileByIdAsync(int fileId);
        Task AddFileAsync(ReportTaskFile file);
        Task DeleteFileAsync(int fileId);
    }
}
