using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{   

    [Table("tblPersonalTask")]
    public class PersonalTask
    {
        // ID Công việc cá nhân 
        [Key]
        [Column("iPersonalTaskID")]
        public int PersonalTaskID { get; set; }

        // ID người dùng 
        [Column("iUserID")]
        public int UserID { get; set; }

        // Tên của công việc cá nhân
        [StringLength(100, ErrorMessage = "Tên công việc không được quá 100 ký tự.")]
        [Required(ErrorMessage = "Tên công việc không được bỏ trống.")]
        [Column("sPersonalTaskName")]
        public string PersonalTaskName { get; set; }

        // Mô tả của công việc cá nhân
        [StringLength(1000, ErrorMessage = "Mô tả công việc không được quá 1000 ký tự.")]
        [Required(ErrorMessage = "Mô tả công việc không được bỏ trống.")]
        [Column("sPersonalTaskDescription")]
        public string PersonalTaskDescription { get; set; }

        // Trạng thái của công việc cá nhân
        [StringLength(50, ErrorMessage = "Trạng thái công việc không được quá 50 ký tự.")]
        [Required(ErrorMessage = "Trạng thái công việc không được bỏ trống.")]
        [Column("sPersonalTaskStatus")]
        public string PersonalTaskStatus { get; set; }

        // Ngày bắt đầu công việc của người dùng
        [Column("dStartDate")]
        [Required(ErrorMessage = "Ngày bắt đầu công việc không được bỏ trống.")]
        public DateOnly StartDate { get; set; }

        // Ngày kết thúc công việc của người dùng
        [Column("dEndDate")]
        [Required(ErrorMessage = "Ngày kết thúc công việc không được bỏ trống.")]
        public DateOnly EndDate { get; set; }


    }
}
