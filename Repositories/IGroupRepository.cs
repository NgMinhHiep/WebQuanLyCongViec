using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface IGroupRepository
    {
        IEnumerable<Group> GetAllGroups();

        // lay ra nhóm theo id
        Group GetGroupById(int id);

        // lay ra danh sách nhóm theo id người dùng
        IEnumerable<Group> GetGroupsByUserID(int userId);

        IEnumerable<Group> GetGroupsUserIsMember(int userId);

        // thêm nhóm mới
        void AddGroup(Group Group);

        // sửa thông tin nhóm
        void UpdateGroup(Group Group);

        // xóa nhóm
        void DeleteGroup(int id);

        void UpdateLeader(int groupId, int newLeaderId);
    }
}
