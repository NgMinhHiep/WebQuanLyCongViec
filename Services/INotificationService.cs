using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Services
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetNotificationsForUser(int userId);
        void CreateNotification(int userId, string title, string content, string type);
        void MarkNotificationAsRead(int notificationId);
    }
}
