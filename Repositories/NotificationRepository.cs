using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetUserNotificationsByID(int userId)
        {
            return _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedTime)
                .ToList();
        }

        public void AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
        }

        public void MarkAsRead(int notificationId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationID == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
