using DeliveryOrders.Domain;

namespace DeliveryOrders.Application.Interfaces.Repositories
{
    public interface IDeliveryOrdersRepository
    {
        /// <summary>
        /// Получение всех заказов
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetAllOrdersAsync();
        
        /// <summary>
        /// Получение отфильтрованных заказов по району и время оформления заказа
        /// </summary>
        /// <param name="district">Район заказа</param>
        /// <param name="dateFrom">Время оформления заказа</param>
        /// <returns></returns>
        List<Order> GetFilteredOrders(String district, DateTime dateFrom);
    }
}
