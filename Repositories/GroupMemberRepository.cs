using Microsoft.EntityFrameworkCore;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupMemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GroupMember> GetGroupMemberByGroupId(int groupId)
        {
            return _context.GroupMembers
                .Where(tv => tv.GroupID == groupId)
                .Include(tv => tv.User)
                .ToList();
        }

        public GroupMember GetGroupMemberByUserAndGroup(int userId, int groupId)
        {
            return _context.GroupMembers
                           .FirstOrDefault(gm => gm.UserID == userId && gm.GroupID == groupId);
        }

        public bool IsUserInGroup(int userId, int groupId)
        {
            return _context.GroupMembers.Any(gm => gm.UserID == userId && gm.GroupID == groupId);
        }
        public void Add(int userId, int groupId, DateTime groupEntryDate)
        {
            var member = new GroupMember
            {
                UserID = userId,
                GroupID = groupId,
                GroupEntryDate = groupEntryDate
            };
            _context.GroupMembers.Add(member); // them thành viên vào nhóm
            _context.SaveChanges(); // luu vao csdl
        }

        public void Delete(int userId, int groupId)
        {
            var memberGroup = _context.GroupMembers.FirstOrDefault(tv => tv.UserID == userId && tv.GroupID == groupId);

            if(memberGroup != null)
            {
                _context.GroupMembers.Remove(memberGroup);
                _context.SaveChanges();
            }
        }

        public void Add(GroupMember groupMember)
        {
            _context.GroupMembers.Add(groupMember);
            _context.SaveChanges();
        }
    }
}
