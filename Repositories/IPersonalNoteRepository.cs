using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface IPersonalNoteRepository
    {
        IEnumerable<PersonalNote> GetAllPersonalNote();

        // lay ra ghi chú cá nhân theo id
        PersonalNote GetPersonalNoteById(int id);

        // lay ra danh sách ghi chú theo id công việc
        IEnumerable<PersonalNote> GetPersonalNoteByPersonalTaskID(int personalTaskId);



        // thêm ghi chú
        void AddPersonalNote(PersonalNote personalNote);

        // sửa ghi chú
        void UpdatePersonalNote(PersonalNote personalNote);

        // xóa ghi chú
        void DeletePersonalNote(int id);
    }
}
