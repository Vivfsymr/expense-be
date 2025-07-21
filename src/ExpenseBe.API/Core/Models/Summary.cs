namespace ExpenseBe.Core.Models
{
    public class Summary
    {
        public string NameOfYear { get; set; } = string.Empty;
        public decimal TotalYearAmountExpense { get; set; } = 0;
        public decimal TotalYearAmountIncome { get; set; } = 0;
        public List<SummaryMonth> SummaryMonths { get; set; } = new List<SummaryMonth>();
    }

    public class SummaryMonth
    {
        public string NameOfMonth { get; set; } = string.Empty;
        public decimal TotalMonthAmountExpense { get; set; } = 0;
        public decimal TotalMonthAmountIncome { get; set; } = 0;
        public decimal TotalMonthAmountRealExpense { get; set; } = 0;
        public List<EachSummaryMonthOfCategory> EachSummaryMonthOfCategories { get; set; } = new List<EachSummaryMonthOfCategory>();
    }

    public class EachSummaryMonthOfCategory
    {
        public string NameOfCategory { get; set; } = string.Empty;
        public decimal TotalCategoryAmountExpense { get; set; } = 0;
    }
}