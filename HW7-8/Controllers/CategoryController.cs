using HW7_8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HW7_8.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly HW7Context db;

        public CategoryController(ILogger<CategoryController> logger, HW7Context context)
        {
            _logger = logger;
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CategoryError = TempData["CategoryError"] != null ? TempData["CategoryError"].ToString():"";
            TempData["CategoryError"] = "";
            var categories = await db.Categories
                .OrderBy(c => c.Id)
                .ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (db.Expenses.Any(x=>x.CategoryId == id))
            {
                TempData["CategoryError"] = "Данная категория используется";
            }
            else
            {
                db.Categories.Remove(await db.Categories.Where(x => x.Id == id).FirstAsync());
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await db.Categories
                .Where(x => x.Id == id)
                .FirstAsync();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            var item = await db.Categories.Where(x => x.Id == model.Id).FirstAsync();
            item.Name = model.Name;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            if (db.Categories.Any(x => x.Name == model.Name))
            {
                ViewBag.NonUnique = "Данная категория уже существует";
                return View(model);
            }
            else
            {
                db.Categories.Add(new Category
                {
                    Name = model.Name,
                });
                await db.SaveChangesAsync();
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
