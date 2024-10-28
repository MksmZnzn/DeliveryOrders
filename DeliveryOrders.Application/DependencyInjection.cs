using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace DeliveryOrders.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // Регистрируем все обработчики CQRS (команды и запросы)
            services.AddMediatR(opt => opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
