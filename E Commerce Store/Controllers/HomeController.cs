using E_Commerce_Store.Models;
using E_Commerce_Store.Views.Middlewares;
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

        private async Task<Cart?> UserCart()
        {
            var uid = HttpContext.Items[BuyerUidMiddleware.BuyerCookieParam].ToString();
            var cart = await _siteContext.Carts.Where(x => x.Uid == uid)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(p => p.MainImage)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                cart = new Cart() { Uid = uid };
                _siteContext.Carts.Add(cart);
            }
            return cart;
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
            ViewData["Title"] = string.Format("Home | Shop");

            return View();
        }
        [HttpGet("/category/{id}/{url}")]
        public async Task<IActionResult> Category(int id)
        {
            var search = HttpContext.Request.Query["search"].ToString();
            var category = await _siteContext.Categories
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Id == id);
            var products = await _siteContext.Products
                .Include(x => x.MainImage)
                .Where(x => x.Category.Id == category.Id)
                .Where(x => x.Title.ToLower().Contains(search))
                .ToListAsync();
            ViewData["Search"] = search;

            ViewData["Products"] = products;
            
            ViewData["Categories"] = await _siteContext.Categories
                .Include(x => x.Image)
                .Include(x => x.Products)
                .ThenInclude(x => x.MainImage)
                .ToListAsync();

            ViewData["Title"] = string.Format("{0} | Shop", category.Title);

            return View(category);
        }

        [HttpGet("/cart/summary")]
        public async Task<IActionResult> CartSummary()
        {
            return PartialView("~/Views/Home/_CartSummary.cshtml", await UserCart());

        }

        [HttpGet("/product/{id}")]
        public async Task<IActionResult> Product(int id)
        {
            ViewData["Categories"] = await _siteContext.Categories
             .Include(x => x.Image)
             .Include(x => x.Products)
             .ThenInclude(x => x.MainImage)
             .ToListAsync();

            var product = await _siteContext.Products
                .Include(x => x.MainImage)
                .FirstAsync(x => x.Id == id);

            var otherProducts = await _siteContext.Products
              .Include(x => x.MainImage)
              .Where(x=>x.Id != product.Id)
              .Take(6)
              .ToListAsync();
            ViewData["OtherProducts"] = otherProducts;
            ViewData["Title"] = string.Format("{0} | Shop", product.Title);
            return View(product);

        }



        [HttpGet("/cart")]
        public async Task<IActionResult> Cart()
        {
            var cart = await UserCart();
            ViewData["Categories"] = await _siteContext.Categories
             .Include(x => x.Image)
             .Include(x => x.Products)
             .ThenInclude(x => x.MainImage)
             .ToListAsync();


            ViewData["Title"] = string.Format("{0} | Shop", "Cart");

            return View(cart);
        }

        [HttpGet("/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cart = await UserCart();
            ViewData["Categories"] = await _siteContext.Categories
      .Include(x => x.Image)
      .Include(x => x.Products)
      .ThenInclude(x => x.MainImage)
      .ToListAsync();
            ViewData["Title"] = string.Format("{0} | Shop", "Checkout");

            return View(cart);
        }
    }
}