using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories.IRepository
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetUserNotificationsByID(int userId);
        void AddNotification(Notification notification);
        void MarkAsRead(int notificationId);
        void SaveChanges();
    }
}
