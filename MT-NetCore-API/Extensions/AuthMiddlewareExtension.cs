using System;
using Microsoft.AspNetCore.Builder;
using MT_NetCore_API.Middlewares;

namespace MT_NetCore_API.Extensions
{
    public static class AuthMiddlewareExtension
    {
        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthMiddleware>();
        }
    }
}
