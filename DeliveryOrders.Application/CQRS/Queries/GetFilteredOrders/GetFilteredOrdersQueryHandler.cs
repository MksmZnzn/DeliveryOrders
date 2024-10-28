using DeliveryOrders.Application.Interfaces.Repositories;
using DeliveryOrders.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryOrders.Application.CQRS.Queries
{
    public class GetFilteredOrdersQueryHandler : IRequestHandler<GetFilteredOrdersQuery, List<Order>>
    {
        private readonly IDeliveryOrdersRepository _orderRepository;
        private readonly ILogger<GetFilteredOrdersQuery> _logger;

        public GetFilteredOrdersQueryHandler(IDeliveryOrdersRepository orderRepository, ILogger<GetFilteredOrdersQuery> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public Task<List<Order>> Handle(GetFilteredOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Начало обработки запроса на получение отфильтрованных заказов для района {District} с момента {Time}.", request.District, request.Time);
            
            var filteredOrders =  _orderRepository.GetFilteredOrders(request.District, request.Time);
            return Task.FromResult(filteredOrders);
        }
    }
}
