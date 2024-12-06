using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblTaskAssignment")]
    public class TaskAssignment
    {
        // ID thành viên
        [Key]
        [Column("iUserID")]
        public int UserID { get; set; }
        //public User User { get; set; }

        // ID nhóm 
        [Column("iGroupID")]
        public int GroupID { get; set; }
        //public Group Group { get; set; }

        // ID công việc
        [Column("iGroupTaskID")]
        public int GroupTaskID { get; set; }

        public GroupTask GroupTask { get; set; }

        public GroupMember GroupMember { get; set; }
    }
}
