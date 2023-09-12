using E_Commerce_Store.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace E_Commerce_Store.Controllers
{
    public class HomeController : Controller
    {
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
            var categories = new List<Category>()
            {
                
                    new Category
                    {
                        Title = "Clothing"
                    },
                    new Category
                    {
                        Title = "Clothing"
                    },
                    new Category
                    {
                        Title = "Clothing"
                    },
                    new Category
                    {
                        Title = "Clothing"
                    },
                    new Category
                    {
                        Title = "Clothing"
                    },
                    new Category
                    {
                        Title = "Clothing"
                    },
            };

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