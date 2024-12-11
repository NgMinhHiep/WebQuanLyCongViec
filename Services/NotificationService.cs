using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<Notification> GetNotificationsForUser(int userId)
        {
            return _notificationRepository.GetUserNotificationsByID(userId);
        }

        public void CreateNotification(int userId, string title, string msg, string type)
        {
            var notification = new Notification
            {
                UserID = userId,
                Title = title,
                Message = msg,
                Type = type,
                CreatedTime = DateTime.Now,
                IsRead = false
            };
            _notificationRepository.AddNotification(notification);
            _notificationRepository.SaveChanges();
        }

        public void MarkNotificationAsRead(int notificationId)
        {
            _notificationRepository.MarkAsRead(notificationId);
            _notificationRepository.SaveChanges();
        }
    }
}
