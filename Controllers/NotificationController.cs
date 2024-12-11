using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Services;

namespace WebNC_BTL_QLCV.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IActionResult NotificationList()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var notifications = _notificationService.GetNotificationsForUser(userId.Value);
            return View(notifications);
        }

        public IActionResult MarkAsRead(int id)
        {
            _notificationService.MarkNotificationAsRead(id);
            return RedirectToAction("NotificationList");
        }

        private int GetCurrentUserId()
        {
            // Lấy UserId từ session hoặc context
            return int.Parse(User.FindFirst("UserId").Value);
        }
    }
}
