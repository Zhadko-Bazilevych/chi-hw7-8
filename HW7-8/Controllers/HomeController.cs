using HW7_8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Globalization;

namespace HW7_8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HW7Context db;

        public HomeController(ILogger<HomeController> logger, HW7Context context)
        {
            _logger = logger;
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await db.Expenses.Include(y => y.Category)
                .Select(x => new ReadExpensesViewModel
                {
                    Id = x.Id,
                    CategoryName = x.Category.Name,
                    Commentary = x.Commentary,
                    Datetime = x.Datetime,
                    Cost = x.Cost,
                }).OrderByDescending(x => x.Datetime)
                .Take(30)
                .ToListAsync();
            return View(expenses);
        }

        public async Task<IActionResult> Delete(int id)
        {
            db.Expenses.Remove(await db.Expenses.Where(x => x.Id == id).FirstAsync());
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await db.Expenses
                .Where(x=>x.Id == id)
                .Select(x => new EditExpensesViewModel
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Commentary = x.Commentary,
                    Datetime = x.Datetime,
                    Cost = x.Cost,
                }).FirstAsync();

            var categories = db.Categories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
            
            item.SelectCategories = categories;

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditExpensesViewModel model)
        {
            var item = await db.Expenses.Where(x => x.Id == model.Id).FirstAsync();
            item.Cost = Math.Round(model.Cost, 2);
            item.Datetime = model.Datetime;
            item.Commentary = model.Commentary;
            item.CategoryId = model.CategoryId;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            EditExpensesViewModel model = new EditExpensesViewModel();
            var DatetimeWithMS = DateTime.Now;
            model.Datetime = DatetimeWithMS.AddTicks(-(DatetimeWithMS.Ticks % 10000000));

            var categories = db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            model.SelectCategories = categories;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EditExpensesViewModel model)
        {
            var newExpense = new Expense
            {
                Cost = Math.Round(model.Cost, 2),
                Datetime = model.Datetime,
                Commentary = model.Commentary,
                CategoryId = model.CategoryId
            };
            await db.Expenses.AddAsync(newExpense);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}