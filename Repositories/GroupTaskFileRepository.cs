using Microsoft.EntityFrameworkCore;
using System.Formats.Tar;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Repositories
{
    public class GroupTaskFileRepository : IGroupTaskFileRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupTaskFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GroupTaskFile>> GetAllFilesAsync(int taskId)
        {
            return await _context.GroupTaskFiles.Where(f => f.GroupTaskID == taskId).ToListAsync();
        }

        public async Task<GroupTaskFile> GetFileByIdAsync(int fileId)
        {
            return await _context.GroupTaskFiles.FindAsync(fileId);
        }

        public async Task AddFileAsync(GroupTaskFile file)
        {
            _context.GroupTaskFiles.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var taskFile = await _context.GroupTaskFiles.FindAsync(fileId);
            if (taskFile != null)
            {
                _context.GroupTaskFiles.Remove(taskFile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
