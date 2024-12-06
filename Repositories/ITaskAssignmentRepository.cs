using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface ITaskAssignmentRepository
    {
        //lấy ra danh sách phụ trách công việc theo id công việc
        IEnumerable<TaskAssignment> GetTaskAssignmentByTaskId(int taskId);

        IEnumerable<GroupMember> GetGroupMemberByGroupId(int groupId);


        // thêm người phụ trách công việc
        void Add(int userId, int groupId, int grouptaskId);

        // xóa người phụ trách công việc
        void Delete(int userId, int grtask);

        public bool IsMemberAssigned(int taskId, int userId);

        IEnumerable<GroupMember> GetAssignmentMemberByTaskID(int taskId);

    }
}
