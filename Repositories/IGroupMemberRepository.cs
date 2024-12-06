using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface IGroupMemberRepository
    {
        // lay ra thành viên nhóm theo id nhóm
        IEnumerable<GroupMember> GetGroupMemberByGroupId(int groupId);

        GroupMember GetGroupMemberByUserAndGroup(int userId, int groupId);

        // Them thành viên vào nhóm
        void Add(int userId, int groupId, DateTime groupEntryDate);

        // Kiểm tra thành viên có trong nhóm hay chưa
        public bool IsUserInGroup(int userId, int groupId);

        void Add(GroupMember groupMember);
        // xóa thành viên
        void Delete(int userId, int groupId);
    }
}
