using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories.IRepository
{
    public interface IFeedbackTaskRepository
    {
        void AddFeedback(FeedbackTask feedback);
        IEnumerable<FeedbackTask> GetFeedbacksByReportFileId(int reportFileId);
        void DeleteFeedback(int id);
        FeedbackTask GetFeedbackById(int id);
        void UpdateFeedback(FeedbackTask feedback);
    }
}
