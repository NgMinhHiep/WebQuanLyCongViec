using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table ("tblNotification")]
    public class Notification
    {
        // ID thông báo
        [Key]
        [Column("iNotificationID")]
        public int NotificationID { get; set; }

        // ID người dùng
        [Column("iUserID")]
        public int UserID { get; set; }

        // Tiêu đề thông báo
        [StringLength(200)]
        [Required]
        [Column("sTitle")]
        public string Title { get; set; }

        // Nội dung thông báo
        [Required]
        [Column("sMessage")]
        public string Message { get; set; }

        // Ngày tạo thông báo
        [Column("dCreatedTime")]
        public DateTime CreatedTime { get; set; }

        // Trạng thái thông báo
        [Column("bIsRead")]
        public Boolean IsRead { get; set; }

        // Loại thông báo
        [Required]
        [Column("sType")]
        public string Type { get; set; }
    }
}
