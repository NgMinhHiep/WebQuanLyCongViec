using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories.IRepository
{
    public interface IParentGroupTaskRepository
    {
        IEnumerable<ParentGroupTask> GetAllParentGroupTask();

        // lay ra công việc nhóm theo id
        ParentGroupTask GetParentGroupTaskById(int id);

        // lay ra danh sách công việc nhóm theo id nhóm
        IEnumerable<ParentGroupTask> GetParentGroupTasksByGroupID(int groupId);

        // thêm công việc nhóm
        void AddParentGroupTask(ParentGroupTask ParentGroupTask);

        // sửa công việc nhóm
        void UpdateParentGroupTask(ParentGroupTask ParentGroupTask);

        // xóa công việc nhóm
        void DeleteParentGroupTask(int id);
    }
}
