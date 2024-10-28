using DeliveryOrders.Domain;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DeliveryOrders.Application.CQRS.Queries
{
    public class GetFilteredOrdersQuery : IRequest<List<Order>>
    {
        /// <summary>
        /// Район доставки
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Время доставки
        /// </summary>
        public DateTime Time { get; set; }
        
        public GetFilteredOrdersQuery() { }

        public GetFilteredOrdersQuery(string district, DateTime fromTime)
        {
            District = district;
            Time = fromTime; 
        }
    }
}