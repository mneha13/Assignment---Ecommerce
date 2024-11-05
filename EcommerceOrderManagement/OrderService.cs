using Ecommerce.Products;
using Ecommerce.Users.Customer;
using Ecommerce.Orders;
using Ecommerce.Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using Ecommerce.Users;
using System.IO;

namespace Ecommerce.Services.OrderServices
{
    public class OrderService
    {
        private readonly Repository<Order> _orderRepository;
        public delegate void OrderProcessedEventHandler(object sender, OrderProcessedEventArgs e);
        public event OrderProcessedEventHandler OrderProcessed;
        private static int _orderIdCounter = 1;

        public OrderService(Repository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public void Add(Order order)
        {
            _orderRepository.Add(order);
        }
        public int IsValidUserChoice(int startRange, int endRange)
        {
            bool isValidInput = false, isValidRange = false;
            string userChoice = "";
            while (!isValidInput || !isValidRange)
            {
                System.Console.WriteLine("Enter  choice");
                userChoice = System.Console.ReadLine();
                isValidInput = int.TryParse(userChoice, out _);
                if (isValidInput)
                {
                    isValidRange = System.Convert.ToInt32(userChoice) >= startRange && System.Convert.ToInt32(userChoice) <= endRange;
                }
                if (!isValidInput || !isValidRange)
                {
                    Console.WriteLine("Invalid choice input. Please try again.");
                }
            }
            return System.Convert.ToInt32(userChoice);
        }

        public void DisplayAllOrders()
        {
           if(_orderRepository.CountItems()==0) throw new OrderNotFoundException(); 
           System.Console.WriteLine("""
                    1 : View orders customer wise
                    2 : View all orders
                    3 : Exit
           """);
           int choice = IsValidUserChoice(1, 3);
           if (choice == 3) return;
           var orders = _orderRepository.GetAll().ToList();
           if (choice == 1)
           {
               var categorizedOrders = orders.GroupBy(o => o.CustomerId);
               var allcategorizedOrder = new List<Order>();
               foreach (var group in categorizedOrders)
               {
                    var orderList = group.ToList();
                    allcategorizedOrder.AddRange(orderList);
               }
               PaginatedData.Paginate(allcategorizedOrder, 3);
           }
           else
           {
               PaginatedData.Paginate(_orderRepository.GetAll().ToList(), 2);
           }
        }

        public int GetOrderQuantity(string name , ProductService productService)
        {
            String quantity = "";
            bool isValidQty = false, isAvailableQty = false;
            while (!isValidQty || !isAvailableQty)
            {
                System.Console.WriteLine("Enter product quantity: ");
                quantity = System.Console.ReadLine();
                isValidQty = int.TryParse(quantity, out _);
                if (isValidQty)
                {
                    isAvailableQty = System.Convert.ToInt32(quantity) <= productService.GetQuantity(name);
                }
                if (!isValidQty)
                {
                    Console.WriteLine("Invalid quantity input. Please try again.");
                }
                if (!isAvailableQty)
                {
                    Console.WriteLine("The quantity you've selected exceeds the available stock");
                }
            }
            return System.Convert.ToInt32(quantity);
        }

        public async Task<int> PlaceOrderAsync(ProductService productService,string name,int id , Address address,int qty)
        {
            await Task.Delay(2000);
            int orderId = _orderIdCounter++;
            Order order = new Order()
            {
                OrderId = orderId ,
                OrderedAt = DateTime.Now , 
                ProductName = name , 
                Status = OrderStatus.Pending , 
                CustomerId = id,
                CustomerAddress=address,Quantity = qty
            };
            Add(order);
            OnOrderProcessed(order);
            order.Status = OrderStatus.Processed;
            return orderId;
        }

        public async Task placeOrder(ProductService productService, int userId,Customer user)
        {
            var products = productService.GetAllProducts();
            if(products == null)
            {
                System.Console.WriteLine("No Products to place order , try again later");
                return;
            }
            System.Console.WriteLine("Choose Product , enter choice");
            foreach (var product in products)
            {
              System.Console.WriteLine($"{product.ProductID} : {product.ProductName}");  
            }
            System.Console.WriteLine($"{products.Count()+1} : To Go Back (Exit)");
            int choice = IsValidUserChoice(1, products.Count() + 1);
            if(choice == products.Count() + 1)
            {
               return;
            }
             string name = productService.GetProductNameById(choice);
             Address address = custAddressForOrder(user);
             try
             {
                 int quantity = GetOrderQuantity(name, productService);
                 int total = productService.GetOrderAmount(quantity, name);
                 System.Console.WriteLine($"The total amount for this order is {total}");
                 int id = await PlaceOrderAsync(productService, name, userId, address, quantity);
                 System.Console.WriteLine($"Order placed for {name} with ID: {id}");
                 productService.SetQuantity(name, quantity);
                 System.Console.WriteLine("Processing...");
             }
             catch (Exception ex)
             {
                 System.Console.WriteLine(ex.Message);
             }
        }

        public Address customerNewAddress(string street , string city , long? zipcode)
        {
            System.Console.WriteLine("Add new address ");

            System.Console.WriteLine("Enter Street ");
            street = System.Console.ReadLine();
            while (string.IsNullOrWhiteSpace(street))
            {
                System.Console.WriteLine("Street cannot be null or empty , Please enter again");
                street = System.Console.ReadLine();
            }

            System.Console.WriteLine("Enter City");
            city = System.Console.ReadLine();
            while (string.IsNullOrWhiteSpace(city))
            {
                System.Console.WriteLine("City cannot be null or empty , Please enter again");
                city = System.Console.ReadLine();
            }
            zipcode = PromptForZipcode();
            Address newAddress = new Address(street, city, (long)zipcode);
            return newAddress;
        }

        public long PromptForZipcode()
        {
            string userInput = "";
            long zipcode;
            while (true)
            {
                System.Console.WriteLine("Enter Zipcode: ");
                 userInput = System.Console.ReadLine();
                try
                {
                    zipcode = Convert.ToInt64(userInput);

                    if (zipcode.ToString().Length == 6)
                        return zipcode;
                    else
                        System.Console.WriteLine("Zip code must be 6 digits.");
                }
                catch (FormatException ex)
                {
                    System.Console.WriteLine("Invalid format. Please enter a numeric zip code.");
                    Logger.LogException(ex);
                }
            }
        }

        public Address custAddressForOrder(Customer user)
        {
            string street="", city="";
            long? zipcode = null;
            Address newAddress = new Address();
            if (user.addresses.Count() > 0)
            {
                int index = 0;
                System.Console.WriteLine("Choose address option for product delivery");
                foreach (var address in user.addresses)
                {
                    System.Console.WriteLine($"{index}:{address}");
                    index++;
                }
                System.Console.WriteLine($"{user.addresses.Count()}:Add new address");
                int choice = IsValidUserChoice(0,user.addresses.Count());
                if (choice == user.addresses.Count())
                {
                    newAddress = customerNewAddress(street, city, zipcode);
                    user.addresses.Add(newAddress);
                    return newAddress;
                }
                else
                {
                    return user.addresses[choice];
                }
            }
            else
            {
                newAddress = customerNewAddress(street, city, zipcode);
                user.addresses.Add(newAddress);
                return newAddress;
            }
        }

        protected void OnOrderProcessed(Order order)
        {
            OrderProcessed?.Invoke(this, new OrderProcessedEventArgs(order));
        }

        public async Task GetOrderStatusAsync(int userId)
        {
             await Task.Run(() =>
            {
                var allOrders = _orderRepository.GetAll();
                var orders = allOrders.Where(o => o.CustomerId == userId).ToList();
                int index = 0;
                System.Console.WriteLine("Enter choice for id");
                foreach(var order in orders)
                {
                    System.Console.WriteLine($"{index} : {order.ProductName} with order id {order.OrderId}");
                    index++;
                }
                System.Console.WriteLine($"{orders.Count()} : To Go Back (Exit)");
                int choice = IsValidUserChoice(0,orders.Count());
                if (choice != orders.Count())
                {
                    Order orderInfo = new Order();
                    int orderId = orders[choice].OrderId;
                    foreach (var order in orders)
                    {
                        if (order.OrderId == orderId)
                        {
                            orderInfo = order;
                        }
                    }
                    if (orderInfo.ProductName != null)
                    {
                        System.Console.WriteLine($"Order ID: {orderId}, Status: {orderInfo.Status}, Product: {orderInfo.ProductName}");
                    }
                    else
                    {
                        System.Console.WriteLine("No order present with this id");
                    }
                }
                else
                {
                    return;
                }

            });
        }

        public void DisplayOrdersByUserId(int userId)
        {
            var orders = _orderRepository.GetAll().Where(o => o.CustomerId.Equals(userId));
            if (orders.Count() > 0)
            {
                System.Console.WriteLine("""
                    1 : View by status
                    2 : View by product name
                    3 : View all
                    4 : Exit
                    """);
                int choice = IsValidUserChoice(1, 4);
                switch (choice) 
                { 
                    case 1:
                        var orderedOrders = orders.OrderBy(order => order.Status);
                        PaginatedData.Paginate(orderedOrders.ToList(), 2);
                        break;
                    case 2:
                        var orderedOrdersByName = orders.OrderBy(order => order.ProductName);
                        PaginatedData.Paginate(orderedOrdersByName.ToList(), 2);
                        break;
                    case 3:
                        PaginatedData.Paginate(orders.ToList(), 2);
                        break;
                    case 4:
                        return;
                }
            }
            else
            {
                throw new Exception("No orders found");
            }
        }

        public void DisplayAllOrdersByStatus()
        {
            var orders = _orderRepository.GetAll();
            var orderedOrders = orders.OrderBy(order => order.Status);
            if (orderedOrders.Count() > 0)
            {
                PaginatedData.Paginate(orderedOrders.ToList(), 3);
            }
            else
            {
                throw new Exception("No orders available to display");
            }
        }

        public void DisplayAllOrdersByOrderDate()
        {
            var orders = _orderRepository.GetAll();
            var orderedOrders = orders.OrderBy(order => order.OrderedAt);
            if (orderedOrders.Count() > 0)
            {
                PaginatedData.Paginate(orderedOrders.ToList(), 3);
            }
            else
            {
                throw new Exception("No orders available to display");
            }
        }

        public void CancelOrder(int id,ProductService productService)
        {
            var orders = _orderRepository.GetAll().Where(o => o.CustomerId.Equals(id) && o.Status != OrderStatus.Cancelled && o.Status != OrderStatus.Delivered).ToList();
            if(orders.Count() == 0)
            {
                System.Console.WriteLine("You do not have any undelivered orders to cancel");
                return;
            }
            int index = 0;
            foreach (var order in orders)
            {
               System.Console.WriteLine($"{index} : {order.ProductName} with order id {order.OrderId}");
               index++;
            }
            System.Console.WriteLine($"{orders.Count()} : To Go Back (Exit)");
            int choice = IsValidUserChoice(0, orders.Count());
            if (choice == orders.Count)
            { 
                return; 
            }
            int orderId = orders[choice].OrderId;
            var cancelOrder = orders[choice];
            productService.UpdateProductQty(cancelOrder.Quantity, cancelOrder.ProductName);
            cancelOrder.Status = OrderStatus.Cancelled;
            System.Console.WriteLine("Order Canceleld");  
        }

        public void UpdateOrderStatus()
        {
            if(_orderRepository.GetAll().Count() == 0)
            {
                System.Console.WriteLine("No order found to update status");
                return;
            }
            var orders = _orderRepository.GetAll().ToList();
            int index = 0;
            foreach (var order in orders)
            {
               System.Console.WriteLine($"{index} : for order id {order.OrderId} with product {order.ProductName} ");
               index++;
            }
            System.Console.WriteLine($"{orders.Count()} : To Go Back (Exit) ");
            int choice = IsValidUserChoice(0,orders.Count());
            if (choice == orders.Count)
            {
                return;
            }
            Order updateOrder = _orderRepository.Find(o => o.OrderId == orders[choice].OrderId);
            if (updateOrder != null)
            {
               System.Console.WriteLine("Enter choice to update status");
               System.Console.WriteLine("""
                        0:Pending
                        1:Processed
                        2:Shipped
                        3:Delivered
                        4:Cancelled
                        5: Exit
               """);
               int choice2 = IsValidUserChoice(0, 5);
               OrderStatus status;
               switch (choice2)
               {
                    case 0: status = OrderStatus.Pending; break;
                    case 1: status = OrderStatus.Processed; break;
                    case 2: status = OrderStatus.Shipped; break;
                    case 3: status = OrderStatus.Delivered; break;
                    case 4: status = OrderStatus.Cancelled; break;
                    case 5: return;
                    default: status = OrderStatus.Pending; break;
               }
               if (choice2 != 5)
               {
                    updateOrder.Status = status;
                    System.Console.WriteLine("Status updated");
               }
            }      
        }
    }
}