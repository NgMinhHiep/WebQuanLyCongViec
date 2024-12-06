using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblGroupMember")]
    public class GroupMember
    {
        // ID thành viên
        [Key]
        [Column("iUserID")]
        public int UserID { get; set; }
        public User User { get; set; }

        // ID nhóm 
        [Column("iGroupID")]
        public int GroupID { get; set; }
        public Group Group { get; set; }

        // Thời gian gia nhập nhóm
        [Column("dGroupEntryDate")]
        public DateTime GroupEntryDate { get; set; }

        public ICollection<TaskAssignment> TaskAssignments { get; set; }
    }
}
