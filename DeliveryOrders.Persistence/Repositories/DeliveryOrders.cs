using DeliveryOrders.Application.Interfaces.Repositories;
using DeliveryOrders.Domain;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Persistence.Configuration;

namespace Persistence.Repositories
{
    public class DeliveryOrdersRepository : IDeliveryOrdersRepository
    {
        private readonly List<Order> _orders;
        private readonly int TimeRangeMinutes = -30;
        private readonly ILogger<DeliveryOrdersRepository> _logger;

        public DeliveryOrdersRepository(FilePathProvider filePathProvider, ILogger<DeliveryOrdersRepository> logger)
        {
            _logger = logger;
            string filePath = filePathProvider.GetOrdersFilePath();

            if (!File.Exists(filePath))
            {
                _logger.LogError("Файл orders.xml не найден по пути: {FilePath}", filePath);
                throw new FileNotFoundException("Файл orders.xml не найден.", filePath);
            }

            _logger.LogInformation("Загрузка заказов из файла {FilePath}", filePath);
            var loader = new OrderXmlLoader();
            _orders = loader.LoadOrdersFromXml(filePath);
        }

        public Task<List<Order>> GetAllOrdersAsync() => Task.FromResult(_orders);

        public List<Order> GetFilteredOrders(string district, DateTime dateFrom)
        {
            DateTime timeRangeEnd = dateFrom.AddMinutes(TimeRangeMinutes);

            var filteredOrders = _orders
                .Where(o => o.District == district && o.OrderTime <= dateFrom && o.OrderTime >= timeRangeEnd)
                .ToList();

            return filteredOrders;
        }
    }
}
