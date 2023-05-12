using Microsoft.AspNetCore.Mvc.Rendering;

namespace HW7_8.Models
{
    public class ExpensesByMonthViewModel
    {
        public List<string> CategoryNames { get; set; }
        public List<MonthExpenseWithCategories> MonthData { get; set; }

    }

    public class MonthExpenseWithCategories
    {
        public string Month { get; set; }
        public List<double> Expenses { get; set; }

    }

    public class MonthSummaryCost
    {
        public int CategoryId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double Cost { get; set; }
    }
}
