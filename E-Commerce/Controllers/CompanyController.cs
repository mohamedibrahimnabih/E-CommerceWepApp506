using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class CompanyController : Controller
    {
        ApplicationDbContext dbContext = new();

        public IActionResult Index()
        {
            var Companies = dbContext.Companies.Include(e=>e.Products).ToList();

            return View(Companies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                dbContext.Companies.Add(company);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        public IActionResult Edit(int companyId)
        {
            var company = dbContext.Companies.Find(companyId);

            if (company != null)
                return View(model: company);

            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                //Company company = new() { Id = Id, Name = Name };
                dbContext.Companies.Update(company);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        public IActionResult Delete(int companyId)
        {
            dbContext.Companies.Remove(new() { Id = companyId });
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
