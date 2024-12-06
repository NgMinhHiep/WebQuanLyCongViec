using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class PersonalTaskRepository : IPersonalTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonalTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // lấy ra toàn bộ danh sách công việc cá nhân
        public IEnumerable<PersonalTask> GetAllPersonalTasks()
        {
            return _context.PersonalTasks.ToList();
        }

        // lấy ra danh sách công việc cá nhân theo id
        public PersonalTask GetPersonalTaskById(int id)
        {
            return _context.PersonalTasks.Find(id);
        }

        //lấy ra danh sách công việc cá nhân theo id người dùng
        public IEnumerable<PersonalTask> GetPersonalTasksByUserID(int userId)
        {
            return _context.PersonalTasks.Where(t => t.UserID == userId).ToList();
        }

        // thêm mới 1 công việc cá nhân
        public void AddPersonalTask(PersonalTask personalTask)
        {
            _context.PersonalTasks.Add(personalTask);
            _context.SaveChanges();
        }

        // sửa 1 công việc cá nhân
        public void UpdatePersonalTask(PersonalTask personalTask)
        {
            _context.PersonalTasks.Update(personalTask);
            _context.SaveChanges();
        }

        // xóa 1 công việc cá nhân
        public void DeletePersonalTask(int id)
        {
            var personalTask = GetPersonalTaskById(id);
            if (personalTask != null)
            {
                _context.PersonalTasks.Remove(personalTask);
                _context.SaveChanges();
            }
        }
    }
}
