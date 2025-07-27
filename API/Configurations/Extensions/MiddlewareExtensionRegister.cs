using Configurations.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Configurations.Extensions
{
    public static class MiddlewareExtensionRegister
    {
        public static IApplicationBuilder UseMiddlewareExtension(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}