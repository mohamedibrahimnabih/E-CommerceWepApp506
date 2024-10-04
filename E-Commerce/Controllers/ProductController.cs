using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var products = dbContext.Products.Include(e=>e.Category).ToList();

            return View(model: products);
        }

        public IActionResult Create()
        {
            var categories = dbContext.Categories.ToList();

            return View(categories);
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile PhotoUrl)
        {
            var fileName = UploadImg(PhotoUrl);
            if (fileName != null)
            {
                product.PhotoUrl = fileName;
            }
            else
            {
                product.PhotoUrl = " ";
            }

            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int productId)
        {
            var categories = dbContext.Categories.ToList();
            var product = dbContext.Products.Find(productId);

            //ViewData["categories"] = categories;
            ViewBag.categories = categories;

            if (product != null)
                return View(model: product);

            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile PhotoUrl)
        {
            var fileName = UploadImg(PhotoUrl);

            var oldPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", PhotoUrl.FileName);

            if (System.IO.File.Exists(oldPhotoPath))
            {
                System.IO.File.Delete(oldPhotoPath);
            }

            if (fileName != null)
            {
                product.PhotoUrl = fileName;
            }
            else
            {
                product.PhotoUrl = " ";
            }


            dbContext.Products.Update(product);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int productId)
        {
            dbContext.Products.Remove(new() { Id = productId });
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private string? UploadImg(IFormFile PhotoUrl)
        {
            // save in wwwroot
            if (PhotoUrl.Length > 0) // 85896
            {
                var fileName = PhotoUrl.FileName; // "1.png"
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    PhotoUrl.CopyTo(stream);
                }
                return PhotoUrl.FileName;
            }
            return null;
        }
    }
}
