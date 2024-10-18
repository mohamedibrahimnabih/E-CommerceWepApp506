using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();

        //CategoryRepository categoryRepository = new CategoryRepository();

        ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }



        public IActionResult Index()
        {
            // var categories = dbContext.Categories.Include("Product").ToList();
            var categories = categoryRepository.GetAll("Products");

            return View(model: categories);
        }

        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // if categeory in the db or not
                //ModelState.AddModelError();
                //return View(category);

                categoryRepository.Create(category);
                categoryRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(category);


            //TempData["success"] = "Add category successfully";

            //CookieOptions cookieOptions = new CookieOptions();
            //cookieOptions.Expires = DateTime.Now.AddDays(14);

            //Response.Cookies.Append("successCookies", "Add category successfully", cookieOptions);

        }

        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetById(categoryId);

            if (category != null)
                return View(model: category);

            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                //Category category = new() { Id = Id, Name = Name };
                categoryRepository.Edit(category);
                categoryRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetById(categoryId);

            categoryRepository.Delete(category);
            categoryRepository.Commit();
            return RedirectToAction(nameof(Index));
        }
    }
}
