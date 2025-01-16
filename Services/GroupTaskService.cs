using WebNC_BTL_QLCV.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebNC_BTL_QLCV.Services
{
    public class GroupTaskService
    {
        private readonly IGroupTaskRepository _GroupTaskRepository;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly INotificationService _notificationService;

        public GroupTaskService(IGroupTaskRepository GroupTaskRepository, INotificationService notificationService, ITaskAssignmentRepository taskAssignmentRepository)
        {
            _GroupTaskRepository = GroupTaskRepository;
            _notificationService = notificationService;
            _taskAssignmentRepository = taskAssignmentRepository;
        }

        public void NotifyTasksEndingSoon(int days)
        {
            // lấy ra danh sách công việc sắp hết hạn
            var tasks = _GroupTaskRepository.GetTasksEndingSoon(days);
            foreach (var task in tasks)
            {
                //lấy danh sách các thành viên đã được giao việc
                var assignedMembers = _taskAssignmentRepository.GetAssignmentMemberByTaskID(task.GroupTaskID);

                foreach (var member in assignedMembers)
                {
                    // gửi thông báo đến các thành viên
                    string title = "Công việc nhóm sắp hết hạn!";
                    string message = $"Công việc '{task.GroupTaskName}' trong nhóm '{task.Group.GroupName}' sẽ hết hạn vào {task.EndDate}.";
                    string type = "Thông báo thời hạn công việc";
                    _notificationService.CreateNotification(member.UserID, title, message, type);
                }

            }
        }
    }
}
