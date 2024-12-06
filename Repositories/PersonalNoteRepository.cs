using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class PersonalNoteRepository : IPersonalNoteRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonalNoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // lấy ra toàn bộ danh sách ghi chú
        public IEnumerable<PersonalNote> GetAllPersonalNote()
        {
            return _context.PersonalNote.ToList();
        }

        // lấy ra danh sách ghi chú cá nhân theo id
        public PersonalNote GetPersonalNoteById(int id)
        {
            return _context.PersonalNote.Find(id);
        }

        //lấy ra danh sách ghi chú theo id công việc
        public IEnumerable<PersonalNote> GetPersonalNoteByPersonalTaskID(int personalTaskId)
        {
            return _context.PersonalNote.Where(t => t.PersonalTaskID == personalTaskId).ToList();
        }

        // thêm mới 1 ghi chú
        public void AddPersonalNote(PersonalNote personalNote)
        {
            _context.PersonalNote.Add(personalNote);
            _context.SaveChanges();
        }

        // sửa 1 ghi chú
        public void UpdatePersonalNote(PersonalNote personalNote)
        {
            _context.PersonalNote.Update(personalNote);
            _context.SaveChanges();
        }

        // xóa 1 ghi chú
        public void DeletePersonalNote(int id)
        {
            var personalNote = GetPersonalNoteById(id);
            if (personalNote != null)
            {
                _context.PersonalNote.Remove(personalNote);
                _context.SaveChanges();
            }
        }
    }
}
