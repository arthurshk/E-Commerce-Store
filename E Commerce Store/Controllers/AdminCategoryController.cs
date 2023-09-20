using E_Commerce_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Store.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly SiteContext _siteContext;
        private readonly IWebHostEnvironment _environment;

        public AdminCategoryController(SiteContext context, IWebHostEnvironment appEnvironment)
        {
            _siteContext = context;
            _environment = appEnvironment;
        }
        [Route("/admin/category/index")]
        public IActionResult Index()
        {
            return View(_siteContext.Categories.Include(x=>x.Image).ToList());
        }
        [HttpGet("/admin/category/create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Admin: Create category";
            return View(new Category());
        }
        [HttpPost("/admin/category/create")]
        public IActionResult Create([FromForm] Category category, IFormFile? image)
        {
            ViewData["Title"] = "Admin: Create category";
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if(image != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var writer = new FileStream(Path.Combine(directoryPath, filename), FileMode.Create))
                {
                    image.CopyTo(writer);
                }
                category.Image = new Image()
                {
                    Filename = filename
                };
                _siteContext.Images.Add(category.Image);
            }
            _siteContext.Categories.Add(category);
            _siteContext.SaveChanges();
            return Redirect("/admin/category/index");
        }
        [HttpGet("/admin/category/edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Admin: Edit category";
            return View(_siteContext.Categories.First(x => x.Id == id));
        }
        [HttpPost("/admin/category/edit/{id}")]
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
            return Redirect("/admin/category/index");
        }
    }
}
