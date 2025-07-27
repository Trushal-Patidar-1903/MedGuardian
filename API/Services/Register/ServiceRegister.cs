using Microsoft.Extensions.DependencyInjection;
using Services.Interface.Masters.BloodGroup;
using Services.Interface.Masters.City;
using Services.Interface.Masters.Country;
using Services.Interface.Masters.Gender;
using Services.Interface.Masters.State;
using Services.Interface.User;
using Services.Service.BloodGroup;
using Services.Service.Gender;
using Services.Service.Masters.City;
using Services.Service.Masters.Country;
using Services.Service.Masters.State;
using Services.Service.User;

namespace Services.Register
{
    public static class ServiceRegister
    {
        public static void AddService(this IServiceCollection Services)
        {
            Services.AddTransient<IUserService, UserService>();
            Services.AddTransient<IBloodGroupService, BloodGroupService>();
            Services.AddTransient<IGenderService, GenderService>();
            Services.AddTransient<IUserMedicalDataService, UserMedicalDataService>();
            Services.AddTransient<ICountryService, CountryService>();
            Services.AddTransient<IStateService, StateService>();
            Services.AddTransient<ICityService, CityService>();
        }
    }
}
