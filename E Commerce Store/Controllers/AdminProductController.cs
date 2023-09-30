using E_Commerce_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Store.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly SiteContext _siteContext;
        private readonly IWebHostEnvironment _environment;

        public AdminProductController(SiteContext context, IWebHostEnvironment appEnvironment)
        {
            _siteContext = context;
            _environment = appEnvironment;
        }
        [Route("/admin/product/index")]
        public IActionResult Index()
        {
            return View(_siteContext.Products
                .Include(x => x.Category)
                .Include(x=>x.MainImage)
                .Include(x => x.Images)
                .ToList());
        }
        [HttpGet("/admin/product/create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Admin: Create product";
            ViewData["Categories"] = _siteContext.Categories.ToList();
            return View(new Product());
        }
        [HttpPost("/admin/product/create")]
        public IActionResult Create([FromForm] Product product, IFormFile? mainImage, ICollection<IFormFile>? images)
        {
            ViewData["Title"] = "Admin: Create product";
            ViewData["Categories"] = _siteContext.Categories.ToList();
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            if(mainImage != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(mainImage.FileName);
                var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var writer = new FileStream(Path.Combine(directoryPath, filename), FileMode.Create))
                {
                    mainImage.CopyTo(writer);
                }
                product.MainImage = new Image()
                {
                    Filename = filename
                };
                _siteContext.Images.Add(product.MainImage);
            }
            _siteContext.Products.Add(product);
            _siteContext.SaveChanges();
            return Redirect("/admin/product/index");
        }
        [HttpGet("/admin/product/edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Admin: Edit category";
            ViewData["Categories"] = _siteContext.Categories.ToList();
            return View(FindProduct(id));
        }
        private Product FindProduct(int id)
        {
            return _siteContext.Products
                  .Include(x => x.Category)
                  .Include(x => x.MainImage)
                  .Include(x => x.Images)
                  .First(x => x.Id == id);
        }
        [HttpPost("/admin/product/edit/{id}")]
        public IActionResult Edit(int id, [FromForm] Product form, IFormFile? mainImage, ICollection<IFormFile>? images)
        {
            ViewData["Title"] = "Admin: Edit category";
            ViewData["Categories"] = _siteContext.Categories.ToList();

            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var product = FindProduct(id);
            product.Title = form.Title;
            product.Description = form.Description;
            product.Price = form.Price;
            product.CategoryId = form.CategoryId;
            if (mainImage != null)
            {
                if(product.MainImage != null)
                {
                    System.IO.File.Delete(Path.Combine(_environment.WebRootPath, "uploads", product.MainImage.Filename));
                    _siteContext.Images.Remove(product.MainImage);
                }
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(mainImage.FileName);
                using (var writer = new FileStream(Path.Combine(_environment.WebRootPath, "uploads", filename), FileMode.Create))
                {
                    mainImage.CopyTo(writer);
                }
                product.MainImage = new Image()
                {
                    Filename = filename
                };
                _siteContext.Images.Add(product.MainImage);
            }
            _siteContext.SaveChanges();
            return Redirect("/admin/product/index");
        }
        [HttpPost("/admin/product/delete/{Id}")]
        public IActionResult Delete(int id)
        {
            var product = _siteContext.Products.FirstOrDefault(x => x.Id == id);
            _siteContext.Products.Remove(product);
            _siteContext.SaveChanges();
            return Redirect("/admin/product/index");
        }
    }
}
