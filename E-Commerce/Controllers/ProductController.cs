using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var categories = dbContext.Categories.ToList().Select(e=> new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            //ViewBag.categories = categories;
            ViewData["categories"] = categories;

            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile PhotoUrl)
        {
            //ModelState.Remove("PhotoUrl");

            if (ModelState.IsValid)
            {
                if (PhotoUrl.Length > 0) // 85896
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoUrl.FileName); // "1.png"
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        PhotoUrl.CopyTo(stream);
                    }

                    product.PhotoUrl = fileName;

                    //return PhotoUrl.FileName;
                }

                //if (fileName != null)
                //{
                //    product.PhotoUrl = fileName;
                //}
                //else
                //{
                //    product.PhotoUrl = " ";
                //}

                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }


            // save photo in wwwroot 


            if(PhotoUrl!= null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoUrl.FileName); // "1.png"
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\temp", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    PhotoUrl.CopyTo(stream);
                }

                product.PhotoUrl = filePath;
            }

            var categories = dbContext.Categories.ToList();
            ViewBag.categories = categories;
            return View(product);
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
             var oldProduct = dbContext.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);
            ModelState.Remove("PhotoUrl");
            //TryValidateModel(product);
            if(ModelState.IsValid)
            {
                if (PhotoUrl != null && PhotoUrl.Length > 0) // 85896
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoUrl.FileName); // "0283dasda2032-321321983lkjwlkds.png"

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.PhotoUrl);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        PhotoUrl.CopyTo(stream);
                    }

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    product.PhotoUrl = fileName;
                }
                else
                {
                    product.PhotoUrl = oldProduct.PhotoUrl;
                }

                dbContext.Products.Update(product);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }


            product.PhotoUrl = oldProduct.PhotoUrl;
            var categories = dbContext.Categories.ToList();
            ViewBag.categories = categories;
            return View(product);
        }

        public IActionResult Delete(int productId)
        {
            var oldProduct = dbContext.Products.Find(productId);

            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldProduct.PhotoUrl);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

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
