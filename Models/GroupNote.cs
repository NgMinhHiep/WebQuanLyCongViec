using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblGroupNote")]
    public class GroupNote
    {
        // ID Ghi chú nhóm
        [Key]
        [Column("iGroupNoteID")]
        public int GroupNoteID { get; set; }

        // ID công việc nhóm 
        [Column("iGroupTaskID")]
        public int GroupTaskID { get; set; }

        // Nội dung của ghi chú nhóm
        [StringLength(1000, ErrorMessage = "Nội dung ghi chú không được quá 1000 ký tự.")]
        [Required(ErrorMessage = "Nội dung ghi chú không được bỏ trống.")]
        [Column("sGroupNoteDetails")]
        public string GroupNoteDetails { get; set; }

        // Thời gian tạo ghi chú
        [Column("dGroupNote_CreationDate")]
        public DateTime GroupNote_CreationDate { get; set; }
    }
}
