using System;
using Ecommerce.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class OrderProcessedEventArgs : EventArgs
    {
        public Order Order { get; }

        public OrderProcessedEventArgs(Order order)
        {
            Order = order;
        }

    }
}
