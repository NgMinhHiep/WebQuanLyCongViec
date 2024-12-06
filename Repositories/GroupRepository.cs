using Microsoft.EntityFrameworkCore;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // lấy ra toàn bộ danh sách nhóm
        public IEnumerable<Group> GetAllGroups()
        {
            return _context.Group.ToList();
        }

        // lấy ra danh sách nhóm theo id
        public Group GetGroupById(int id)
        {
            return _context.Group.Find(id);
        }

        //lấy ra danh sách nhóm theo id người dùng mà người dùng là trưởng nhóm
        public IEnumerable<Group> GetGroupsByUserID(int userId)
        {
            return _context.Group.Where(t => t.LeaderID == userId).ToList();
        }

        public IEnumerable<Group> GetGroupsUserIsMember(int userId)
        {
            return _context.GroupMembers.Include(tv => tv.Group) // bao gồm thông tin nhóm
                                        .Where(tv => tv.UserID == userId)
                                        .Select(tv => tv.Group) // lấy danh sách nhóm
                                        .ToList();
        }

        // thêm mới 1 nhóm
        public void AddGroup(Group Group)
        {
            _context.Group.Add(Group);
            _context.SaveChanges();
        }

        // sửa 1 nhóm
        public void UpdateGroup(Group Group)
        {
            _context.Group.Update(Group);
            _context.SaveChanges();
        }

        // xóa 1 nhóm
        public void DeleteGroup(int id)
        {
            //var groupTasks = _context.GroupTasks.Where(gt => gt.GroupID == id).ToList();
            //var groupTasks = _context.GroupTasks.Include(gt => gt.TaskAssignments).FirstOrDefault(gt => gt.GroupTaskID == id);
            //_context.TaskAssignments.RemoveRange(groupTasks.TaskAssignments);
            //_context.Remove(groupTasks);

            var Group = GetGroupById(id);
            if (Group != null)
            {
                _context.Group.Remove(Group);
                _context.SaveChanges();
            }
        }
    }
}
