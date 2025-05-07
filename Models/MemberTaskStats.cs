namespace WebNC_BTL_QLCV.Models
{
    public class MemberTaskStats
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int NotStartedTasks { get; set; }

        public double CompletedPercentage { get; set; }
        public double InProgressPercentage { get; set; }
        public double NotStartedPercentage { get; set; }
    }
}
