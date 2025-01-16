using WebNC_BTL_QLCV.Repositories;

namespace WebNC_BTL_QLCV.Services
{
    public class PersonalTaskService
    {
        private readonly IPersonalTaskRepository _personalTaskRepository;
        private readonly INotificationService _notificationService;

        public PersonalTaskService(IPersonalTaskRepository personalTaskRepository, INotificationService notificationService)
        {
            _personalTaskRepository = personalTaskRepository;
            _notificationService = notificationService;
        }

        public void NotifyTasksEndingSoon(int days)
        {
            var tasks = _personalTaskRepository.GetTasksEndingSoon(days);
            foreach (var task in tasks)
            {
                string title = "Công việc sắp hết hạn!";
                string message = $"Công việc '{task.PersonalTaskName}' sẽ hết hạn vào {task.EndDate}.";
                string type = "Thông báo thời hạn công việc";
                _notificationService.CreateNotification(task.UserID, title, message, type);
            }
        }
    }
}
