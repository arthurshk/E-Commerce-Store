using E_Commerce_Store.Models;
using E_Commerce_Store.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Store.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly SiteContext _siteContext;
        public readonly ImageStorage _imageStorage;

        public AdminCategoryController(SiteContext context, ImageStorage imageStorage)
        {
            _siteContext = context;
            _imageStorage = imageStorage;
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
        public async Task<IActionResult> Create([FromForm] Category category, IFormFile? image)
        {
            ViewData["Title"] = "Admin: Create category";
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if(image != null)
            {
                category.Image = await _imageStorage.UploadAsync(image);
            
            }
           await _siteContext.Categories.AddAsync(category);
          await  _siteContext.SaveChangesAsync();
            return Redirect("/admin/category/index");
        }
        [HttpGet("/admin/category/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Admin: Edit category";
            return View(await _siteContext.Categories.FirstAsync(x => x.Id == id));
        }
        [HttpPost("/admin/category/edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Category form, IFormFile? image)
        {
            ViewData["Title"] = "Admin: Edit category";
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var category = await _siteContext.Categories.Include(x => x.Image).FirstAsync(x => x.Id == id);
            category.Title = form.Title;
            category.Url = form.Url;
            if (image != null)
            {
                if(category.Image != null)
                {
                    _imageStorage.Remove(category.Image);
                }
                category.Image = await _imageStorage.UploadAsync(image);
           
            }
          await  _siteContext.SaveChangesAsync();
            return Redirect("/admin/category/index");
        }
    }
}
