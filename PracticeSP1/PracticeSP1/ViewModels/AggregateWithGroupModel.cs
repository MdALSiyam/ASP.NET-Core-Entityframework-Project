namespace PracticeSP1.ViewModels
{
    public class AggregateWithGroupModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal AvgValue { get; set; }
        public decimal SumValue { get; set; }
        public int CountValue { get; set; }
    }

    public class AggregateWithSummaryModel
    {
        public decimal MinSummary { get; set; }
        public decimal MaxSummary { get; set; }
        public decimal AvgSummary { get; set; }
        public decimal SumSummary { get; set; }
        public int CountSummary { get; set; }
        public ICollection<AggregateWithGroupModel> AggregateResult { get; set; }
    }
}
