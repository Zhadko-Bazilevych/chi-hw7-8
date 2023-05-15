using HW7_8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;

namespace HW7_8.Controllers
{
    public class StatisticController : Controller
    {
        private readonly ILogger<StatisticController> _logger;
        private readonly HW7Context db;

        public StatisticController(ILogger<StatisticController> logger, HW7Context context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetByMonth()
        {
            var Table = new ExpensesByMonthViewModel();
            Table.MonthData = new List<MonthExpenseWithCategories>();
            Table.CategoryNames = await db.Categories.OrderBy(x => x.Id).Select(s => s.Name).ToListAsync();

            var list = (await db.Categories.ToListAsync())
                .SelectMany(c => db.Expenses.Select(e => new { Category = c, e.Datetime.Year, e.Datetime.Month }).Distinct())
                .GroupJoin((await db.Expenses.ToListAsync()),
                           x => new { Id = x.Category.Id, Year = x.Year, Month = x.Month },
                           e => new { Id = e.CategoryId, Year = e.Datetime.Year, Month = e.Datetime.Month },
                           (x, g) => new { x.Category.Id, x.Year, x.Month, TotalCost = g.Sum(e => e.Cost) })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenBy(x => x.Id)
                .Select(x=> new MonthSummaryCost
                {
                    CategoryId = x.Id,
                    Month = x.Month,
                    Year = x.Year,
                    Cost = x.TotalCost
                })
                .ToList();

            int tempMonth = -1;
            for(int i = 0; i< list.Count(); i++)
            {
                if(tempMonth != list[i].Month)
                {
                    Table.MonthData.Add(new MonthExpenseWithCategories
                    {
                        Month = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(list[i].Month)} {list[i].Year}",
                        Expenses = new List<double>(),
                    });
                    tempMonth = list[i].Month;
                }
                Table.MonthData.Last().Expenses.Add(Math.Round(list[i].Cost, 2));
            }

            return View(Table);
        }
    }

    class check
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
