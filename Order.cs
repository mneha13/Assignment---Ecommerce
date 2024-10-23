using Ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Orders
{
    public class Order
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderedAt { get; set; }
        public string ProductName { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public Address CustomerAddress { get; set; }

        public void GetStatus(Order order)
        {
            System.Console.WriteLine($"The order status for order id{order.OrderId} is {order.Status}");
        }

        public void DisplayOrderInfo(Order order)
        {
            System.Console.WriteLine($""" 
                    **Order Information** 
                    Id = {order.OrderId}
                    Status = {order.Status}
                    Ordered At = {order.OrderedAt.ToString()}
                    Product Name = {order.ProductName}
                    Quantity = {order.Quantity}
                    Customer Id = {order.CustomerId}
                    Customer Address = {order.CustomerAddress}
                    """);
        }
    }
}
