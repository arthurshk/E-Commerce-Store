namespace E_Commerce_Store.Views.Middlewares
{
    public class BuyerUidMiddleware
    {
        private readonly RequestDelegate _next;
        public static string BuyerCookieParam = "BuyerUID";
        public BuyerUidMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var uid = context.Request.Cookies[BuyerCookieParam];
            if (uid == null)
            {
                uid = Guid.NewGuid().ToString();
                context.Response.Cookies.Append(BuyerCookieParam, uid, new CookieOptions
                {
                    Expires = new DateTimeOffset(2038, 1,1,0,0,0,TimeSpan.FromHours(0))
                });
            }
            context.Items[BuyerCookieParam] = uid;

            await _next(context);
        }
    }

    public static class BuyerUidMiddlewareExtension
    {
        public static IApplicationBuilder UseBuyerUid(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BuyerUidMiddleware>();
        }
    }
}