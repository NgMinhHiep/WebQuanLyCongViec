using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Repositories
{
    public class FeedbackTaskRepository : IFeedbackTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbackTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // thêm phản hồi
        public void AddFeedback(FeedbackTask feedback)
        {
            _context.FeedbackTasks.Add(feedback);
            _context.SaveChanges();
        }

        // lấy danh sách feedback bằng id báo cáo
        public IEnumerable<FeedbackTask> GetFeedbacksByReportFileId(int reportFileId)
        {
            return _context.FeedbackTasks
                .Where(f => f.ReportTaskFileID == reportFileId)
                .OrderByDescending(f => f.FeedbackTime)
                .ToList();
        }

        //lấy phản hồi bằng id
        public FeedbackTask GetFeedbackById(int id)
        {
            return _context.FeedbackTasks.Find(id);
        }

        // xóa phản hồi
        public void DeleteFeedback(int id)
        {
            var feedback = _context.FeedbackTasks.Find(id);
            if (feedback != null)
            {
                _context.FeedbackTasks.Remove(feedback);
                _context.SaveChanges();
            }
        }

        public void UpdateFeedback(FeedbackTask feedback)
        {
            _context.FeedbackTasks.Update(feedback);
            _context.SaveChanges();
        }
    }
}
