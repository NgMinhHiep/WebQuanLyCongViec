using Microsoft.EntityFrameworkCore;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class GroupTaskRepository : IGroupTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // lấy ra toàn bộ danh sách công việc nhóm
        public IEnumerable<GroupTask> GetAllGroupTask()
        {
            return _context.GroupTasks.ToList();
        }

        // lấy ra danh sách công việc nhóm theo id
        public GroupTask GetGroupTaskById(int id)
        {
            return _context.GroupTasks.Find(id);
        }

        //lấy ra danh sách công việc nhóm theo id nhóm
        public IEnumerable<GroupTask> GetGroupTaskByGroupID(int groupId)
        {
            return _context.GroupTasks.Where(gt => gt.GroupID == groupId)
                .Include(gt => gt.TaskAssignments)
                    .ThenInclude(ta => ta.GroupMember)
                    .ThenInclude(gm => gm.User)
                .ToList();
        }

        // tìm kiếm người phụ trách công việc qua id group và id công việc
        public TaskAssignment GetUsersByGroupIDAndGroupTaskID(int groupId, int taskId)
        {
            return _context.TaskAssignments
                .FirstOrDefault(ta => ta.GroupID == groupId && ta.GroupTaskID == taskId);
        }

        // thêm mới 1 công việc
        public void AddGroupTask(GroupTask GroupTask)
        {
            _context.GroupTasks.Add(GroupTask);
            _context.SaveChanges();
        }

        // sửa 1 công việc
        public void UpdateGroupTask(GroupTask GroupTask)
        {
            _context.GroupTasks.Update(GroupTask);
            _context.SaveChanges();
        }

        // xóa 1 công việc
        public void DeleteGroupTask(int id)
        {
            var taskAssignments = _context.TaskAssignments.Where(gt => gt.GroupTaskID == id).ToList();
            foreach (var taskAssignment in taskAssignments)
            {
                _context.TaskAssignments.RemoveRange(taskAssignment);
            }

            var GroupTask = GetGroupTaskById(id);
            if (GroupTask != null)
            {
                _context.GroupTasks.Remove(GroupTask);
                _context.SaveChanges();
            }
        }
    }
}
