using E_Commerce_Store.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Store.Views.Middlewares
{
    public class CartProductsCountMiddleware
    {
        private readonly RequestDelegate _next;
        public CartProductsCountMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var uid = context.Items[BuyerUidMiddleware.BuyerCookieParam].ToString();
            var db = context.RequestServices.GetService<SiteContext>(); 
            var cart = await db.Carts
                .Where(x => x.Uid == uid)
                .Include(x => x.Products)
                .FirstOrDefaultAsync();
            context.Items["CartProductsCount"] = cart != null? cart.Products.Count() : 0;
            await _next(context);
        }
    }

    public static class CartProductsCountMiddlewareExtension
    {
        public static IApplicationBuilder UseCartProductsCount(this IApplicationBuilder builder)
        {
           return builder.UseWhen(context =>
           {
               return !context.Request.Path.StartsWithSegments("/admin");

           }, appBuilder =>
           {
               appBuilder.UseMiddleware<CartProductsCountMiddleware>();
           });
        }
    }
}