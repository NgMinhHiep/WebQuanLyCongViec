using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface IGroupTaskRepository
    {
        IEnumerable<GroupTask> GetAllGroupTask();

        // lay ra công việc nhóm theo id
        GroupTask GetGroupTaskById(int id);

        // lay ra danh sách công việc nhóm theo id cv cha
        IEnumerable<GroupTask> GetGroupTasksByParentGroupTaskID(int parenttaskid);

        // thêm công việc nhóm
        void AddGroupTask(GroupTask GroupTask);

        // sửa công việc nhóm
        void UpdateGroupTask(GroupTask GroupTask);

        // xóa công việc nhóm
        void DeleteGroupTask(int id);

        IEnumerable<GroupTask> GetTasksEndingSoon(int days);

        void IncrementLateCount(int taskId);
        void ExtendDeadline(int taskId, DateOnly newEndDate);

        public List<MemberTaskStats> GetMemberTaskStatsByGroupId(int groupId);
    }
}

