namespace PracticeSP1.ViewModels
{
    public class AggragateViewModel
    {
        public decimal MinValue { get; set; }
        public decimal AvgValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal SumValue { get; set; }
        public int TotalCount { get; set; }
        public ICollection<GroupByViewModel> GroupResult { get; set; }

    }

    public class GroupByViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int CountGroup { get; set; }
        public decimal MinGroup { get; set; }
        public decimal MaxGroup { get; set; }
        public decimal SumGroup { get; set; }
        public decimal AvgGroup { get; set; }
    }
}
