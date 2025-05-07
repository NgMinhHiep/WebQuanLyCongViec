using WebNC_BTL_QLCV.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebNC_BTL_QLCV.Repositories.IRepository;

namespace WebNC_BTL_QLCV.Services
{
    public class GroupTaskService
    {
        private readonly IGroupTaskRepository _GroupTaskRepository;
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;

        public GroupTaskService(IGroupTaskRepository GroupTaskRepository, INotificationService notificationService, INotificationRepository notificationRepository)
        {
            _GroupTaskRepository = GroupTaskRepository;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
        }

        public void NotifyTasksEndingSoon(int days)
        {
            // lấy ra danh sách công việc sắp hết hạn
            var tasks = _GroupTaskRepository.GetTasksEndingSoon(days);

            var today = DateTime.Today;

            foreach (var task in tasks)
            {
                var userId = task.UserID;
                var groupName = task.ParentGroupTask?.Group?.GroupName ?? "(Không rõ nhóm)";
                var parentTaskName = task.ParentGroupTask?.ParentGroupTaskName ?? "(Không rõ công việc cha)";

                var title = $"Công việc sắp hết hạn: {task.GroupTaskName}";
                var message = $"Công việc \"{task.GroupTaskName}\" (thuộc công việc cha \"{parentTaskName}\" của nhóm \"{groupName}\") " +
                     $"sẽ hết hạn vào ngày {task.EndDate:dd/MM/yyyy}.";

                if (!_notificationRepository.HasSentDeadlineNotificationToday(userId, title))
                {
                    _notificationService.CreateNotification(userId, title, message, "Deadline");
                }
            }

        }
    }
}
