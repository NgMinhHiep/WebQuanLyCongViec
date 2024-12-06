using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblPersonalNote")]
    public class PersonalNote
    {
        // ID Ghi chú cá nhân 
        [Key]
        [Column("iPersonalNoteID")]
        public int PersonalNoteID { get; set; }

        // ID công việc cá nhân 
        [Column("iPersonalTaskID")]
        public int PersonalTaskID { get; set; }

        // Nội dung của ghi chú cá nhân
        [StringLength(1000, ErrorMessage = "Nội dung ghi chú không được quá 1000 ký tự.")]
        [Required(ErrorMessage = "Nội dung ghi chú không được bỏ trống.")]
        [Column("sPersonalNoteDetails")]
        public string PersonalNoteDetails { get; set; }

        // Thời gian tạo ghi chú
        [Column("dPersonalNote_CreationDate")]
        public DateTime PersonalNote_CreationDate { get; set; }
    }
}
