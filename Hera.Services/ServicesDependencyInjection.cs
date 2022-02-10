using Hera.Common.Core;
using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Hera.Data.Repositories;
using Hera.Services.Businesses;
using Microsoft.Extensions.DependencyInjection;

namespace Hera.Services
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddHeraServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServiceBaseTypeId<,>), typeof(ServiceBaseTypeId<,>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddTransient(typeof(IRepositoryBaseTypeId<,>), typeof(RepositoryBaseTypeId<,>));
            services.AddTransient(typeof(IRepositoryBaseTypeId<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }   
    }
}
