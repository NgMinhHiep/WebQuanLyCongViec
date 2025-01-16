using Microsoft.EntityFrameworkCore;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskAssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskAssignment> GetTaskAssignmentByTaskId(int taskId)
        {
            return _context.TaskAssignments
                .Where(ta => ta.GroupTaskID == taskId)
                .Include(ta => ta.GroupMember.User)
                .ToList();
        }

        public IEnumerable<GroupMember> GetGroupMemberByGroupId(int groupId)
        {
            return _context.GroupMembers
                .Where(tv => tv.GroupID == groupId)
                .Include(tv => tv.User)
                .ToList();
        }

        public void Add(int userId, int groupId, int grouptaskId)
        {
            var taskAssignment = new TaskAssignment
            {
                UserID = userId,
                GroupID = groupId,
                GroupTaskID = grouptaskId
            };
            _context.TaskAssignments.Add(taskAssignment); // them thành viên vào nhóm
            _context.SaveChanges(); // luu vao csdl
        }
        
        public void Delete(int userId, int grtask)
        {
            var taskAssignment = _context.TaskAssignments.FirstOrDefault(tv => tv.UserID == userId && tv.GroupTaskID == grtask);

            if (taskAssignment != null)
            {
                _context.TaskAssignments.Remove(taskAssignment);
                _context.SaveChanges();
            }
        }
        
        public bool IsMemberAssigned(int taskId, int userId)
        {
            return _context.TaskAssignments.Any(ta => ta.GroupTaskID == taskId && ta.UserID == userId);
        }


        public IEnumerable<GroupMember> GetAssignmentMemberByTaskID(int taskId)
        {
            var assignemntMembers = _context.TaskAssignments
                                    .Where(ta => ta.GroupTaskID == taskId)
                                    .Include(ta => ta.GroupMember)          // Bao gồm GroupMember trước
                                    .ThenInclude(gm => gm.User)         // Bao gồm User thông qua GroupMember
                                    .Select(ta => ta.GroupMember)
                                    .ToList();
            return assignemntMembers;
        }

    }
}
