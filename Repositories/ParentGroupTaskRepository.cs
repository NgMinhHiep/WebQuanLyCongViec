using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories.IRepository
{
    public class ParentGroupTaskRepository : IParentGroupTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public ParentGroupTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // lấy ra toàn bộ danh sách công việc nhóm
        public IEnumerable<ParentGroupTask> GetAllParentGroupTask()
        {
            return _context.ParentGroupTasks.ToList();
        }

        // lấy ra danh sách công việc nhóm theo id
        public ParentGroupTask GetParentGroupTaskById(int id)
        {
            return _context.ParentGroupTasks.Find(id);
        }

        //lấy ra danh sách công việc nhóm theo id nhóm
        public IEnumerable<ParentGroupTask> GetParentGroupTasksByGroupID(int groupId)
        {
            //return _context.ParentGroupTasks.Where(gt => gt.GroupID == groupId).ToList();
            return _context.ParentGroupTasks
                .Where(gt => gt.GroupID == groupId)
                .AsEnumerable()
                .OrderBy(task =>
                {
                    if (task.PriorityLevel != null && task.PriorityLevel.StartsWith("Số"))
                    {
                        var parts = task.PriorityLevel.Split(' ');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int level))
                        {
                            return level;
                        }
                    }
                    return int.MaxValue;
                })
                .ToList();
        }
        

        // thêm mới 1 công việc cha
        public void AddParentGroupTask(ParentGroupTask ParentGroupTask)
        {
            _context.ParentGroupTasks.Add(ParentGroupTask);
            _context.SaveChanges();
        }

        // sửa 1 công việc cha
        public void UpdateParentGroupTask(ParentGroupTask ParentGroupTask)
        {
            _context.ParentGroupTasks.Update(ParentGroupTask);
            _context.SaveChanges();
        }

        // xóa 1 công việc cha
        public void DeleteParentGroupTask(int id)
        {


            var ParentGroupTask = GetParentGroupTaskById(id);
            if (ParentGroupTask != null)
            {
                _context.ParentGroupTasks.Remove(ParentGroupTask);
                _context.SaveChanges();
            }
        }
    }
}
