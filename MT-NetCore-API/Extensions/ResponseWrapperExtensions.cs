using System;
using Microsoft.AspNetCore.Builder;
using MT_NetCore_API.Middlewares;

namespace MT_NetCore_API.Extensions
{
    public static class ResponseWrapperExtensions
    {
        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseWrapper>();
        }
    }
}
