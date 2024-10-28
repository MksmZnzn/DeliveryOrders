using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DeliveryOrders.Domain;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Path.Combine(AppContext.BaseDirectory, "../../../.."); 
            string filePath = Path.Combine(rootPath, "orders.xml");

            GenerateSampleOrdersXml(filePath);
            Console.WriteLine($"XML файл с данными заказов успешно создан: {filePath}");
        }

        static void GenerateSampleOrdersXml(string filePath)
        {
            var orders = new List<Order>();
            var random = new Random();
            DateTime firstOrderTime = DateTime.Now;

            for (int i = 0; i < 40; i++)
            {
                orders.Add(new Order
                {
                    OrderId = Guid.NewGuid(),
                    OrderTime = firstOrderTime.AddMinutes(i * -5),
                    District = $"District{random.Next(1, 5)}",
                    Weight = Math.Round(random.NextDouble() * 10, 2)
                });
            }

            XDocument xdoc = new XDocument(
                new XElement("Orders",
                    orders.Select(order => 
                        new XElement("Order",
                            new XElement("OrderId", order.OrderId),
                            new XElement("DeliveryTime", order.OrderTime.ToString("yyyy-MM-ddTHH:mm:ss")),
                            new XElement("District", order.District),
                            new XElement("Weight", order.Weight)
                        )
                    )
                )
            );

            xdoc.Save(filePath);
        }
    }
}