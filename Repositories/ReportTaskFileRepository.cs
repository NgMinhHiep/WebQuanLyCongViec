using Microsoft.EntityFrameworkCore;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Repositories
{
    public class ReportTaskFileRepository : IReportTaskFileRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportTaskFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReportTaskFile>> GetAllFilesAsync(int taskId)
        {
            return await _context.ReportTaskFiles.Where(f => f.GroupTaskID == taskId).ToListAsync();
        }

        // lấy ra danh sách toàn bộ file báo cáo của 1 công việc cha
        public IEnumerable<ReportTaskFile> GetReportFilesByParentTaskId(int parentTaskId)
        {
            return _context.ReportTaskFiles
                .Include(r => r.GroupTask)
                .Where(r => r.GroupTask.ParentGroupTaskID == parentTaskId)
                .ToList();
        }

        public async Task<ReportTaskFile> GetFileByIdAsync(int fileId)
        {
            return await _context.ReportTaskFiles.FindAsync(fileId);
        }

        public async Task AddFileAsync(ReportTaskFile file)
        {
            _context.ReportTaskFiles.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var taskFile = await _context.ReportTaskFiles.FindAsync(fileId);
            if (taskFile != null)
            {
                _context.ReportTaskFiles.Remove(taskFile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
