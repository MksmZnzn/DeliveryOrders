using DeliveryOrders.Application.Interfaces.Repositories;
using DeliveryOrders.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryOrders.Application.CQRS.Queries
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<Order>>
    {
        private readonly IDeliveryOrdersRepository _deliveryOrdersRepository;
        private readonly ILogger<GetAllOrdersQuery> _logger;
            
        public GetAllOrdersQueryHandler(IDeliveryOrdersRepository deliveryOrdersRepository, ILogger<GetAllOrdersQuery> logger)
        {
            _deliveryOrdersRepository = deliveryOrdersRepository;
            _logger = logger;
        }

        public async Task<List<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Начало обработки запроса на получение всех заказов");
            
            return await _deliveryOrdersRepository.GetAllOrdersAsync();
        }
    }
}
