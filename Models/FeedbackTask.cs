using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblFeedbackTask")]
    public class FeedbackTask
    {
        // ID phản hồi báo cáo
        [Key]
        [Column("iFeedbackTaskID")]
        public int FeedbackTaskID { get; set; }

        // nội dung phản hồi 
        [StringLength(1000, ErrorMessage = "Nội dung phản hồi không được quá 1000 ký tự.")]
        [Required(ErrorMessage = "Nội dung phản hồi không được bỏ trống.")]
        [Column("sFeedbackDetails")]
        public string FeedbackDetails { get; set; }

        // Thoi gian phản hồi
        [Column("dFeedbackTime")]
        public DateTime FeedbackTime { get; set; }

        // ID báo cáo
        [Column("iReportTaskFileID")]
        public int ReportTaskFileID { get; set; }
    }
}
