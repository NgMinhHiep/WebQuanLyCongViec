using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblParentGroupTask")]
    public class ParentGroupTask
    {
        // ID Công việc cha 
        [Key]
        [Column("iParentGroupTaskID")]
        public int ParentGroupTaskID { get; set; }

        // ID nhóm 
        [Column("iGroupID")]
        public int GroupID { get; set; }

        // Tên của công việc cha
        [StringLength(100, ErrorMessage = "Tên công việc không được quá 100 ký tự.")]
        [Required(ErrorMessage = "Tên công việc không được bỏ trống.")]
        [Column("sParentGroupTaskName")]
        public string ParentGroupTaskName { get; set; }

        // Mô tả của công việc cha
        [StringLength(1000, ErrorMessage = "Mô tả công việc không được quá 1000 ký tự.")]
        [Required(ErrorMessage = "Mô tả công việc không được bỏ trống.")]
        [Column("sParentGroupTaskDescription")]
        public string ParentGroupTaskDescription { get; set; }

        // Trạng thái của công việc cha
        [StringLength(50, ErrorMessage = "Trạng thái công việc không được quá 50 ký tự.")]
        [Required(ErrorMessage = "Trạng thái công việc không được bỏ trống.")]
        [Column("sParentGroupTaskStatus")]
        public string ParentGroupTaskStatus { get; set; }

        // Mức độ ưu tiên của công việc cha
        [StringLength(50, ErrorMessage = "Độ ưu tiên công việc không được quá 50 ký tự.")]
        [Required(ErrorMessage = "Độ ưu tiên công việc không được bỏ trống.")]
        [Column("sPriorityLevel")]
        public string PriorityLevel { get; set; }

        public ICollection<GroupTask> GroupTasks { get; set; } = new List<GroupTask>();
        
        [ValidateNever]
        public Group Group { get; set; } 
    }
}
