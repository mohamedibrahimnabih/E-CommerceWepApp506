using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var categories = dbContext.Categories.ToList();

            return View(model: categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);

            if(category != null)
                return View(model: category);

            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            //Category category = new() { Id = Id, Name = Name };
            dbContext.Categories.Update(category);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int categoryId)
        {
            dbContext.Categories.Remove(new() { Id = categoryId });
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
