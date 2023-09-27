using E_Commerce_Store.Models;
using E_Commerce_Store.Models.Forms;
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
        private async Task<Cart?> UserCart()
        {
            var uid = HttpContext.Items[BuyerUidMiddleware.BuyerCookieParam].ToString();
            var cart = await _siteContext.Carts.Where(x => x.Uid == uid)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                cart = new Cart() { Uid = uid };
                _siteContext.Carts.Add(cart);
            }
            return cart;
        }

        [HttpDelete("/api/remove-from-cart")]
        public async Task<IActionResult> RemoveFromCart([FromBody] int productId)
        {
            var cart = await UserCart();
            var productToRemove = cart.Products.FirstOrDefault(x => x.Id == productId);
            if (productToRemove != null)
            {
                cart.Products.Remove(productToRemove);
                return Ok(new { Ok = true });
            } else
            {
                cart.Products.Remove(productToRemove);
                return NotFound(new { Ok = false });
            }
        }

        [HttpPut("/api/add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddProductRequest addProduct)
        {
            var cart = await UserCart();
          
            var product = await _siteContext.Products.FirstAsync(x => x.Id == addProduct.ProductId);

            var cartProduct = cart.Products.Where(x => x.Product.Id == product.Id).FirstOrDefault();
            if(cartProduct == null)
            {
                cart.Products.Add(new CartProduct
                {
                    Product = product,
                    Price = product.Price,
                    Quantity = addProduct.Quantity,
                });
            }
            else
            {
                cartProduct.Quantity += addProduct.Quantity;
            }
            await _siteContext.SaveChangesAsync();

            return Ok(new
            {
                CartProductsCount = cart.Products.Count,
            });
        }
    
    }
}
