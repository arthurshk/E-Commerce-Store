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
            return View(_siteContext.Categories.First(x => x.Id == id));
        }
        [HttpPost("/admin/product/edit/{id}")]
        public IActionResult Edit(int id, [FromForm] Category form, IFormFile? image)
        {
            ViewData["Title"] = "Admin: Edit category";
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var category = _siteContext.Categories.Include(x => x.Image).First(x => x.Id == id);
            category.Title = form.Title;
            category.Url = form.Url;
            if (image != null)
            {
                if(category.Image != null)
                {
                    System.IO.File.Delete(Path.Combine(_environment.WebRootPath, "uploads", category.Image.Filename));
                    _siteContext.Images.Remove(category.Image);
                }
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                using (var writer = new FileStream(Path.Combine(_environment.WebRootPath, "uploads", filename), FileMode.Create))
                {
                    image.CopyTo(writer);
                }
                category.Image = new Image()
                {
                    Filename = filename
                };
                _siteContext.Images.Add(category.Image);
            }
            _siteContext.SaveChanges();
            return Redirect("/admin/product/index");
        }
    }
}
