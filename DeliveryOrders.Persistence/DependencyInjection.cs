using Microsoft.Extensions.DependencyInjection;
using DeliveryOrders.Application.Interfaces.Repositories;
using Persistence.Configuration;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddSingleton<FilePathProvider>();
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDeliveryOrdersRepository, DeliveryOrdersRepository>();
            return services;
        }
    }
}
