using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PersonalTask> PersonalTasks { get; set; }
        public DbSet<PersonalNote> PersonalNote { get; set; }
        public DbSet<Group> Group{ get; set; }
        public DbSet<GroupTask> GroupTasks { get; set; }
        public DbSet<GroupNote> GroupNotes { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.UserID, gm.GroupID });

            modelBuilder.Entity<GroupMember>()
                .HasOne(tv => tv.User)
                .WithMany(nd => nd.GroupMembers)
                .HasForeignKey(tv => tv.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMember>()
                .HasOne(tv => tv.Group)
                .WithMany(n => n.GroupMembers)
                .HasForeignKey(tv => tv.GroupID)
                .OnDelete(DeleteBehavior.Cascade);

            /*
            modelBuilder.Entity<GroupTask>()
                .HasOne(gt => gt.Group)
                .WithMany(g => g.GroupTasks)
                .HasForeignKey(gt => gt.GroupID)
                .OnDelete(DeleteBehavior.NoAction);
            */
            modelBuilder.Entity<TaskAssignment>().HasKey(ta => new { ta.UserID, ta.GroupID, ta.GroupTaskID });

            
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.GroupTask)
                .WithMany(gt => gt.TaskAssignments)
                .HasForeignKey(ta => ta.GroupTaskID)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.GroupMember)
                .WithMany(gr => gr.TaskAssignments)
                .HasForeignKey(ta => new { ta.UserID, ta.GroupID })
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
