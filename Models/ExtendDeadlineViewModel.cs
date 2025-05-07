using System.ComponentModel.DataAnnotations;

namespace WebNC_BTL_QLCV.Models
{
    public class ExtendDeadlineViewModel
    {
        public int GroupTaskID { get; set; }
        public DateOnly CurrentEndDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly NewEndDate { get; set; }
    }
}
