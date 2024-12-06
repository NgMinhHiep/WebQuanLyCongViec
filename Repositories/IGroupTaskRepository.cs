using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface IGroupTaskRepository
    {
        IEnumerable<GroupTask> GetAllGroupTask();

        // lay ra công việc nhóm theo id
        GroupTask GetGroupTaskById(int id);

        // lay ra danh sách công việc nhóm theo id nhóm
        IEnumerable<GroupTask> GetGroupTaskByGroupID(int groupId);

        TaskAssignment GetUsersByGroupIDAndGroupTaskID(int groupId, int taskId);

        // thêm công việc nhóm
        void AddGroupTask(GroupTask GroupTask);

        // sửa công việc nhóm
        void UpdateGroupTask(GroupTask GroupTask);

        // xóa công việc nhóm
        void DeleteGroupTask(int id);
    }
}

