using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
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

        //lấy ra danh sách công việc nhóm theo id công việc cha
        public IEnumerable<GroupTask> GetGroupTasksByParentGroupTaskID(int parenttaskid)
        {
            //return _context.GroupTasks.Where(gt => gt.ParentGroupTaskID == parenttaskid).ToList();
            return _context.GroupTasks
                .Where(gt => gt.ParentGroupTaskID == parenttaskid)
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
        /*
        public IEnumerable<GroupTask> GetGroupTaskByGroupID(int groupId)
        {
            return _context.GroupTasks.Where(gt => gt.GroupID == groupId)
                .Include(gt => gt.TaskAssignments)
                    .ThenInclude(ta => ta.GroupMember)
                    .ThenInclude(gm => gm.User)
                .ToList();
        }
        */
        // tìm kiếm người phụ trách công việc qua id group và id công việc


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
           

            var GroupTask = GetGroupTaskById(id);
            if (GroupTask != null)
            {
                _context.GroupTasks.Remove(GroupTask);
                _context.SaveChanges();
            }
        }
        
        public IEnumerable<GroupTask> GetTasksEndingSoon(int days)
        {
            var today = DateOnly.FromDateTime(DateTime.Now); // Lấy ngày hôm nay
            var endDateThreshold = today.AddDays(days); // Ngày kết thúc sau X ngày

            return _context.GroupTasks
                .Include(gt => gt.ParentGroupTask) // Lấy thông tin cv cha liên quan
                    .ThenInclude(pgt => pgt.Group)
                .Where(t => t.EndDate > today && t.EndDate <= endDateThreshold)
                .ToList();
        }

        
        public void IncrementLateCount(int taskId)
        {
            var task = _context.GroupTasks.Find(taskId);
            if (task != null)
            {
                task.LateCount++;
                _context.SaveChanges();
            }
        }

        public void ExtendDeadline(int taskId, DateOnly newEndDate)
        {
            var task = _context.GroupTasks.Find(taskId);
            if (task != null)
            {
                task.EndDate = newEndDate;
                task.LateCount++;       // tăng 1 lần quá hạn
                _context.SaveChanges();
            }
        }

        public List<MemberTaskStats> GetMemberTaskStatsByGroupId(int groupId)
        {
            var members = _context.GroupMembers
                .Include(m => m.User)
                .Where(m => m.GroupID == groupId)
                .ToList();

            var groupTasks = _context.GroupTasks
                .Where(gt => gt.UserID != null)
                .ToList();

            var result = members.Select(m =>
            {
                var totalTasks = groupTasks.Count(t => t.UserID == m.UserID);
                var completedTasks = groupTasks.Count(t => t.UserID == m.UserID && t.GroupTaskStatus == "Đã hoàn thành");
                var inProgressTasks = groupTasks.Count(t => t.UserID == m.UserID && t.GroupTaskStatus == "Đang thực hiện");
                var notStartedTasks = groupTasks.Count(t => t.UserID == m.UserID && t.GroupTaskStatus == "Chưa hoàn thành");

                // Tính phần trăm hoàn thành, đang thực hiện và chưa hoàn thành
                double completedPercentage = totalTasks == 0 ? 0 : (double)completedTasks / totalTasks * 100;
                double inProgressPercentage = totalTasks == 0 ? 0 : (double)inProgressTasks / totalTasks * 100;
                double notStartedPercentage = totalTasks == 0 ? 0 : (double)notStartedTasks / totalTasks * 100;

                return new MemberTaskStats
                {
                    UserID = m.UserID,
                    FullName = m.User.FullName,
                    TotalTasks = totalTasks,
                    CompletedTasks = completedTasks,
                    InProgressTasks = inProgressTasks,
                    NotStartedTasks = notStartedTasks,
                    CompletedPercentage = completedPercentage,
                    InProgressPercentage = inProgressPercentage,
                    NotStartedPercentage = notStartedPercentage
                };
            }).ToList();

            /*
            var result = members.Select(m => new MemberTaskStats
            {
                UserID = m.UserID,
                FullName = m.User.FullName,
                TotalTasks = groupTasks.Count(t => t.UserID == m.UserID),
                CompletedTasks = groupTasks.Count(t => t.UserID == m.UserID && t.GroupTaskStatus == "Đã hoàn thành"),
                InProgressTasks = groupTasks.Count(t => t.UserID == m.UserID && t.GroupTaskStatus == "Đang thực hiện"),
                NotStartedTasks = groupTasks.Count(t => t.UserID == m.UserID && t.GroupTaskStatus == "Chưa hoàn thành"),

            }).ToList();
            */

            return result;
        }

    }
}
