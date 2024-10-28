using DeliveryOrders.Domain;
using MediatR;

namespace DeliveryOrders.Application.CQRS.Queries
{
    public class GetAllOrdersQuery : IRequest<List<Order>>
    {

    }
}
