using Microsoft.Extensions.DependencyInjection;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.Masters.BloodGroup;
using Repositories.Interface.Masters.City;
using Repositories.Interface.Masters.Country;
using Repositories.Interface.Masters.Gender;
using Repositories.Interface.Masters.State;
using Repositories.Interface.User;
using Repositories.Repository.Common.Sql;
using Repositories.Repository.Masters.BloodGroup;
using Repositories.Repository.Masters.City;
using Repositories.Repository.Masters.Country;
using Repositories.Repository.Masters.Gender;
using Repositories.Repository.Masters.State;
using Repositories.Repository.User;

namespace Repositories.Register
{
    public static class RepositoryRegister
    {
        public static void AddRepository(this IServiceCollection Services)
        {
            Services.AddScoped<IBaseSqlManager, BaseSqlManager>();
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddScoped<IBloodGroupRepository, BloodGroupRepository>();
            Services.AddScoped<IGenderRepository, GenderRepository>();
            Services.AddScoped<IUserMedicalDataRepository, UserMedicalDataRepository>();
            Services.AddScoped<ICountryRepository, CountryRepository>();
            Services.AddScoped<IStateRepository, StateRepository>();
            Services.AddScoped<ICityRepository, CityRepository>();
        }
    }
}
