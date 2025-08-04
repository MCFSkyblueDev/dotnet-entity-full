using Microsoft.AspNetCore.Antiforgery;

namespace Api.Middlewares
{
    public class AntiforgeryValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IAntiforgery antiforgery)
        {
            if (HttpMethods.IsPost(context.Request.Method) ||
                HttpMethods.IsPut(context.Request.Method) ||
                HttpMethods.IsDelete(context.Request.Method))
            {
                await antiforgery.ValidateRequestAsync(context);
            }

            await _next(context);
        }
    }
}