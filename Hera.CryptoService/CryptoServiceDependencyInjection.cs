using Hera.CryptoService.Services.CoinMarketCapServices;
using Microsoft.Extensions.DependencyInjection;

namespace Hera.CryptoService
{
    public static class CryptoServiceDependencyInjection
    {
        public static IServiceCollection AddHeraCryptoServices(this IServiceCollection services)
        {
            services.AddScoped<ICoinMarketCapService, CoinMarketCapService>();
            services.AddScoped<ListingLatestCMCService, ListingLatestCMCService>();
            return services;
        }
    }
}
