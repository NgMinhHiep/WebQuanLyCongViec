using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public interface IPersonalTaskRepository
    {
        IEnumerable<PersonalTask> GetAllPersonalTasks();

        // lay ra công việc cá nhân theo id
        PersonalTask GetPersonalTaskById(int id);

        // lay ra danh sách công việc cá nhân theo id người dùng
        IEnumerable<PersonalTask> GetPersonalTasksByUserID(int userId);

        // thêm công việc cá nhân
        void AddPersonalTask(PersonalTask personalTask);

        // sửa công việc cá nhân
        void UpdatePersonalTask(PersonalTask personalTask);

        // xóa công việc cá nhân
        void DeletePersonalTask(int id);

        IEnumerable<PersonalTask> GetTasksEndingSoon(int days);
    }
}
