
using System.Formats.Tar;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories.IRepository
{
    public interface IGroupTaskFileRepository
    {
        Task<List<GroupTaskFile>> GetAllFilesAsync(int taskId); // Lấy file theo TaskId
        Task<GroupTaskFile> GetFileByIdAsync(int fileId);
        Task AddFileAsync(GroupTaskFile file);
        Task DeleteFileAsync(int fileId);
    }
}
