using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface IGroupNoteRepository
    {
        IEnumerable<GroupNote> GetAllGroupNote();

        // lay ra ghi chú công việc theo id
        GroupNote GetGroupNoteById(int id);

        // lay ra danh sách ghi chú theo id công việc
        IEnumerable<GroupNote> GetGroupNoteByGroupTaskID(int GroupTaskId);



        // thêm ghi chú
        void AddGroupNote(GroupNote GroupNote);

        // sửa ghi chú
        void UpdateGroupNote(GroupNote GroupNote);

        // xóa ghi chú
        void DeleteGroupNote(int id);
    }
}
