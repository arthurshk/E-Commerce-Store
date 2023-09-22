using E_Commerce_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace E_Commerce_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly SiteContext _siteContext;
        public HomeController(SiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        [HttpGet("/home")]
        public async Task<IActionResult> Index()
        {
            var products = _siteContext
                .Products
                .Include(x => x.MainImage)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Take(6)
                .ToList();
            var categories = _siteContext.Categories
                .Include(x => x.Image)
                .ToList();

            ViewData["Products"] = products;
            ViewData["Categories"] = categories;
            ViewData["NewProducts"] = products;
            return View();
        }
        [HttpGet("/category/{id}/{url}")]
        public async Task<IActionResult> Category(int id)
        {
            var category = await _siteContext.Categories
                .Include(x => x.Image)
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            ViewData["Categories"] = await _siteContext.Categories
                .Include(x => x.Image)
                .Include(x => x.Products)
                .ThenInclude(x => x.MainImage)
                .ToListAsync();
            return View(category);
        }
    }
}