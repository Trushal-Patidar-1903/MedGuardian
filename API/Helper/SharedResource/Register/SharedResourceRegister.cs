using Helper.SharedResource.Interface.File;
using Helper.SharedResource.Interface.Jwt;
using Helper.SharedResource.Interface.Utility;
using Helper.SharedResource.Service.File;
using Helper.SharedResource.Service.Jwt;
using Helper.SharedResource.Service.SharedResource;
using Microsoft.Extensions.DependencyInjection;

namespace Helper.SharedResource.Register
{
    public static class SharedResourceRegister
    {
        public static void AddSharedResources(this IServiceCollection Services)
        {
            Services.AddTransient<IJwtTokenServices, JwtTokenServices>();
            Services.AddTransient<IUtilityServices, UtilityServices>();
            Services.AddTransient<IFileService, FileService>();
        }
    }
}
