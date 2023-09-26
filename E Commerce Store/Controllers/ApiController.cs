using E_Commerce_Store.Models;
using E_Commerce_Store.Views.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Store.Controllers
{
    public class ApiController : ControllerBase
    {
        private readonly SiteContext _siteContext;
        public ApiController(SiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        [HttpPut("/api/add-to-cart/{id}")]
        public async Task<IActionResult> AddToCart(int id)
        {
            var uid = HttpContext.Items[BuyerUidMiddleware.BuyerCookieParam].ToString();
            var cart = await _siteContext.Carts.Where(x => x.Uid == uid)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .FirstAsync();
            if(cart == null)
            {
                cart = new Cart() { Uid = uid };
                _siteContext.Carts.Add(cart);
            }
            var product = await _siteContext.Products.FirstAsync(x => x.Id == id);

            var cartProduct = cart.Products.Where(x => x.Product.Id == product.Id).FirstOrDefault();
            if(cartProduct == null)
            {
                cart.Products.Add(new CartProduct
                {
                    Product = product,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                cartProduct.Quantity++;
            }
            await _siteContext.SaveChangesAsync();

            return Ok();
        }
    }
}
