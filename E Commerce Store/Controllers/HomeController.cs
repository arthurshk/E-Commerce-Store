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
            var products = new List<Product>()
            {
                new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                 new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                  new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                   new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                    new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                     new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                      new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                       new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
                        new Product
                {
                    Title = "Product",
                    Price = 125.99M,
                    Description = "Light and comfortable"
                },
            };
            var categories = _siteContext.Categories
                .Include(x => x.Image)
                .ToList();

            ViewData["Products"] = products;
            ViewData["Categories"] = categories;
            ViewData["NewProducts"] = products;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}