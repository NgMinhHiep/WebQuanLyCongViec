using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblGroupTaskFile")]
    public class GroupTaskFile
    {
        // ID File công việc nhóm 
        [Key]
        [Column("iGroupTaskFileID")]
        public int GroupTaskFileID { get; set; }

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
        [Column("sFileName")]
        public string FileName { get; set; }

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
    }
}
