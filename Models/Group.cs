using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblGroup")]
    public class Group
    {
        // ID nhóm
        [Key]
        [Column("iGroupID")]
        public int GroupID { get; set; }

        // ID trưởng nhóm 
        [Column("iLeaderID")]
        public int LeaderID { get; set; }

        // Tên nhóm
        [StringLength(200, ErrorMessage = "Tên nhóm không được quá 200 ký tự.")]
        [Required(ErrorMessage = "Tên nhóm không được bỏ trống.")]
        [Column("sGroupName")]
        public string GroupName { get; set; }

        // Thời gian tạo nhóm
        [Column("dGroupFormationDate")]
        public DateOnly GroupFormationDate { get; set; }

        public ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
        public ICollection<GroupTask> GroupTasks { get; set; } = new List<GroupTask>();


        [Column("sVerifyKey")]
        [Required(ErrorMessage = "KEy không được bỏ trống.")]
        [StringLength(10, ErrorMessage = "Key không được quá 10 ký tự.")]
        [MinLength(6, ErrorMessage = "Key phải có ít nhất 6 ký tự.")]
        [RegularExpression(@".*\d$", ErrorMessage = "Key phải kết thúc bằng một số.")]
        public string VerifyKey { get; set; }
    }
}
