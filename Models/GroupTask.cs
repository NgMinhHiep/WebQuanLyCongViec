using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblGroupTask")]
    public class GroupTask
    {
        // ID Công việc nhóm 
        [Key]
        [Column("iGroupTaskID")]
        public int GroupTaskID { get; set; }

        // ID công việc cha  
        [Column("iParentGroupTaskID")]
        public int ParentGroupTaskID { get; set; }

        // Tên của công việc nhóm
        [StringLength(100, ErrorMessage = "Tên công việc không được quá 100 ký tự.")]
        [Required(ErrorMessage = "Tên công việc không được bỏ trống.")]
        [Column("sGroupTaskName")]
        public string GroupTaskName { get; set; }

        // Mô tả của công việc nhóm
        [StringLength(1000, ErrorMessage = "Mô tả công việc không được quá 1000 ký tự.")]
        [Required(ErrorMessage = "Mô tả công việc không được bỏ trống.")]
        [Column("sGroupTaskDescription")]
        public string GroupTaskDescription { get; set; }

        // Trạng thái của công việc nhóm
        [StringLength(50, ErrorMessage = "Trạng thái công việc không được quá 50 ký tự.")]
        [Required(ErrorMessage = "Trạng thái công việc không được bỏ trống.")]
        [Column("sGroupTaskStatus")]
        public string GroupTaskStatus { get; set; }

        // Ngày bắt đầu công việc của nhóm
        [Column("dStartDate")]
        [Required(ErrorMessage = "Ngày bắt đầu công việc không được bỏ trống.")]
        public DateOnly StartDate { get; set; }

        // Ngày kết thúc công việc của nhóm
        [Column("dEndDate")]
        [Required(ErrorMessage = "Ngày kết thúc công việc không được bỏ trống.")]
        public DateOnly EndDate { get; set; }

        // Mức độ ưu tiên của công việc nhóm
        [StringLength(50, ErrorMessage = "Độ ưu tiên công việc không được quá 50 ký tự.")]
        [Required(ErrorMessage = "Độ ưu tiên công việc không được bỏ trống.")]
        [Column("sPriorityLevel")]
        public string PriorityLevel { get; set; }

        // Id thành viên phụ trách công việc
        [Column("iUserID")]
        public int UserID { get; set; }

        // số lần công việc hết hạn
        [Column("iLateCount")]
        public int LateCount { get; set; } = 0;

        [ForeignKey("ParentGroupTaskID")]
        [ValidateNever]
        public ParentGroupTask ParentGroupTask { get; set; }
        //public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    }
}
