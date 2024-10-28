using DeliveryOrders.Domain;
using System.Globalization;
using System.Xml.Linq;

public class OrderXmlLoader
{
    public List<Order> LoadOrdersFromXml(string filePath)
    {
        var orders = new List<Order>();
        XDocument xdoc = XDocument.Load(filePath);
    
        foreach (XElement orderElement in xdoc.Descendants("Order"))
        {
            if (Guid.TryParse(orderElement.Element("OrderId")?.Value, out Guid orderId) &&
                DateTime.TryParse(orderElement.Element("DeliveryTime")?.Value, out DateTime orderTime) &&
                orderElement.Element("District") != null &&
                double.TryParse(orderElement.Element("Weight")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double weight))
            {
                var order = new Order
                {
                    OrderId = orderId,
                    OrderTime = orderTime,
                    District = orderElement.Element("District").Value,
                    Weight = weight
                };
                orders.Add(order);
            }
            else
            {
                // Обработка ошибок парсинга, например, логирование или выброс исключения
                throw new FormatException($"Ошибка формата данных для заказа: {orderElement}");
            }
        }
        return orders;
    }
}