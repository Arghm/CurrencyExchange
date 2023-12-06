using CurrencyExchange.Application.Handlers;
using CurrencyExchange.Application.Repositories;
using CurrencyExchange.Contracts.Handlers;
using CurrencyExchange.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Application.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCurrencyExchangeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserHandler, UserHandler>();
            services.AddScoped<ICurrencyHandler, CurrencyHandler>();
            services.AddScoped<IUserAccountHandler, UserAccountHandler>();

            services.AddTransient<IDbRepository, DbRepository>();

            return services;
        }
    }
}
