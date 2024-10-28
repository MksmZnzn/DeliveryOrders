namespace DeliveryOrders.Domain
{
    public class Order
    {
        /// <summary>
        /// ID заказа
        /// </summary>
        public Guid OrderId { get; set; }
        
        /// <summary>
        /// Вес заказа
        /// </summary>
        public double Weight { get; set; }
        
        /// <summary>
        /// Район
        /// </summary>
        public string? District { get; set; }
        
        /// <summary>
        /// Время оформления заказа
        /// </summary>
        public DateTime OrderTime { get; set; }
    }
}
