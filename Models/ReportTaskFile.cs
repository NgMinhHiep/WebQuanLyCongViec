using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblReportTaskFile")]
    public class ReportTaskFile
    {
        // ID File báo cáo công việc 
        [Key]
        [Column("iReportTaskFileID")]
        public int ReportTaskFileID { get; set; }

        // ID công việc nhóm
        [Column("iGroupTaskID")]
        public int GroupTaskID { get; set; }

        // Tên nguoi gui
        [StringLength(255, ErrorMessage = "Tên người gửi không được quá 255 ký tự.")]
        [Column("sSenderName")]
        public string SenderName { get; set; }

        // Tên file
        [StringLength(255, ErrorMessage = "Tên file không được quá 255 ký tự.")]
        [Required(ErrorMessage = "Tên file không được bỏ trống.")]
        [Column("sReportFileName")]
        public string ReportFileName { get; set; }

        // ID file ggdrive 
        [StringLength(255, ErrorMessage = "Tên file ggdrive không được quá 255 ký tự.")]
        [Required(ErrorMessage = "Tên file ggdrive không được bỏ trống.")]
        [Column("sGoogleDriveFileId")]
        public string GoogleDriveFileId { get; set; }


        // Kich thuoc file
        [Column("fFileSize")]
        public double FileSize { get; set; }


        // Loai file
        [Column("sFileType")]
        public string FileType { get; set; }


        // Thoi gian upload file
        [Column("dUploadedTime")]
        public DateTime UploadedTime { get; set; }

        // Mô tả file
        [Column("sReportDescription")]
        public string ReportDescription { get; set; }

        [ValidateNever]
        public GroupTask GroupTask { get; set; }
    }
}
