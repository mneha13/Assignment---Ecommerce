using Ecommerce.Products;
using Ecommerce.Users;
using Ecommerce.Users.Customer;
using Ecommerce.Orders;
using Ecommerce.PwdBuilder;
using Ecommerce.Services.ProductServices;
using Ecommerce.Services.OrderServices;
using Ecommerce.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;
namespace Ecommerce
{
    public class EcommerceOrderManagementSystem
    {
        public static int productIdCounter = 1,UserIdCounter=1;
        private static void OnOrderProcessed(object sender, OrderProcessedEventArgs e)
        {
            Console.WriteLine($"Notification: Order {e.Order.OrderId} has been processed successfully!");
        }

        public static int NavigateMenu(string option)
        {
            System.Console.WriteLine($"""
                0 : Go Back
                1 : Continue with {option}
                """);
            String userChoice = "";
            bool isValidInput = false, isValidRange = false;
            while (!isValidInput || !isValidRange)
            {
                System.Console.WriteLine("Enter  choice");
                userChoice = System.Console.ReadLine();
                isValidInput = int.TryParse(userChoice, out _);
                if (isValidInput)
                {
                    isValidRange = System.Convert.ToInt32(userChoice) == 0 || System.Convert.ToInt32(userChoice) == 1;
                }
                if (!isValidInput || !isValidRange)
                {
                    Console.WriteLine("Invalid choice input. Please try again.");
                }
            }
            int choice = System.Convert.ToInt32(userChoice);
            return choice;
        }

        public static void Main()
        {
            Console.WriteLine("Ecommerce Order Management System");
            var dbConnection = DatabaseConnection.Instance;
            var productRepository = dbConnection.GetRepository<Product>();
            var orderRepository = dbConnection.GetRepository<Order>();
            var userRepository = dbConnection.GetRepository<User>();
            var serviceProvider = new ServiceCollection()
                .AddTransient<Product>()
                .BuildServiceProvider();
            var productService = new ProductService(productRepository, serviceProvider);
            var orderService = new OrderService(orderRepository);
            var userService = new UserService(userRepository);
            orderService.OrderProcessed += OnOrderProcessed;
            UserFactory userFactory = new UserFactory();
            IEnumerable<Product> productsByCategory = new List<Product>();

            int loginChoice=0, currentUserLoggedInId = 0;
            string userName,userPassword = "", currentUserLoggedInRole = Role.CUSTOMER.ToString();
            void HandleCustomer()
            {
                while (true)
                {
                    int choice = -1;
                    System.Console.WriteLine("""
                                    Please enter choice:
                                    1 : Browse Products
                                    2 : Place an order
                                    3 : Check order status
                                    4 : Display products of a specific category
                                    5 : View all orders
                                    6 : Cancel order
                                    7 : Change Password
                                    0 : exit
                                    """);
                    try
                    {
                         choice = System.Convert.ToInt32(System.Console.ReadLine());
                    }
                    catch(Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            int c1 = NavigateMenu("Browse Products");
                            if (c1 == 1)
                            {
                                Console.Clear();
                                productService.DisplayAllProducts();
                            }
                            break;
                        case 2:
                            Console.Clear();
                            int c2 = NavigateMenu("Place Order");
                            if (c2 == 1)
                            {
                                Console.Clear();
                                Customer user = (Customer)userService.GetSingleUser(currentUserLoggedInId);
                                orderService.placeOrder(productService, currentUserLoggedInId, user);
                                Thread.Sleep(3000);
                            }
                            break;
                        case 3:
                            Console.Clear();
                            int c3 = NavigateMenu("Check Order status");
                            if (c3 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    orderService.GetOrderStatusAsync(currentUserLoggedInId).GetAwaiter().GetResult();
                                }

                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 4:
                            Console.Clear();
                            int c4 = NavigateMenu("Display products of a specific category");
                            if (c4 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    productService.GetProductsByCategory();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                }
                            }
                            break;
                        case 5:
                            Console.Clear();
                            int c5 = NavigateMenu("View all orders");
                            if (c5 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    orderService.DisplayOrdersByUserId(currentUserLoggedInId);
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 6:
                            Console.Clear();
                            int c6 = NavigateMenu("Cancel Order");
                            if (c6 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    orderService.CancelOrder(currentUserLoggedInId, productService);
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 7:
                            Console.Clear();
                            int c7 = NavigateMenu("Change Password");
                            if (c7 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    userService.ChangePassword(currentUserLoggedInId);
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 0:
                            return;

                        default:
                            System.Console.WriteLine("Invalid choice");
                            break;
                    }
                }
            }


            void HandleAdmin()
            {
                while (true)
                {
                    int choice = -1;
                    System.Console.WriteLine("""
                                    Please enter choice:
                                    1 : Add Product
                                    2 : Remove Product
                                    3 : View all orders
                                    4 : View all orders by status
                                    5 : View all orders by order date
                                    6 : Display products of a specific category
                                    7 : Update order status
                                    8 : View all products
                                    9 : Manage Inventory(add product quantity)
                                    10 : Update Product Price
                                    11 : Change Password
                                    0 : exit
                                    """);
                    try
                    {
                        choice = System.Convert.ToInt32(System.Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                    
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            int c1 = NavigateMenu("Add Product");
                            if (c1 == 1)
                            {
                                Console.Clear();
                                System.Console.WriteLine("To quit at any point enter \"quit\" to exit");
                                try
                                {
                                    productService.AddProduct(ref productIdCounter,serviceProvider);
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 2:
                            Console.Clear();
                            int c2 = NavigateMenu("Remove Product");
                            if (c2 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    productService.DeleteProduct();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 3:
                            Console.Clear();
                            int c3 = NavigateMenu("View all orders");
                            if (c3 == 1)
                            {
                                Console.Clear();
                                orderService.DisplayAllOrders();
                            }
                            break;
                        case 4:
                            Console.Clear();
                            int c4 = NavigateMenu("View all orders by status");
                            if (c4 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    orderService.DisplayAllOrdersByStatus();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 5:
                            Console.Clear();
                            int c5 = NavigateMenu("View all orders by date");
                            if (c5 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    orderService.DisplayAllOrdersByOrderDate();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 6:
                            Console.Clear();
                            int c6 = NavigateMenu("Display products of specific category");
                            if (c6 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    productService.GetProductsByCategory();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 7:
                            Console.Clear();
                            int c7 = NavigateMenu("Update order status");
                            if (c7 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    orderService.UpdateOrderStatus();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 8:
                            Console.Clear();
                            int c8 = NavigateMenu("View all products");
                            if (c8 == 1)
                            {
                                Console.Clear();
                                productService.DisplayAllProducts();
                            }
                            break;
                        case 9:
                            Console.Clear();
                            int c9 = NavigateMenu("Manage Inventory(add product quantity)");
                            if (c9 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    productService.AddQuantity();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 10:
                            Console.Clear();
                            int c10 = NavigateMenu("Update product price");
                            if (c10 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    productService.UpdatePrice();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 11:
                            Console.Clear();
                            int c11 = NavigateMenu("Change Password");
                            if (c11 == 1)
                            {
                                Console.Clear();
                                try
                                {
                                    userService.ChangePassword(currentUserLoggedInId);
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                    Logger.LogException(ex);
                                }
                            }
                            break;
                        case 0:
                            return;
                        default:
                            System.Console.WriteLine("Invalid choice");
                            break;
                    }
                }
            }



            while (true)
            {
                System.Console.WriteLine("""
                    Enter choice:
                    1 : Login
                    2 : Sign up
                    3 : Exit
                    """);
                try
                {
                    loginChoice = System.Convert.ToInt32(System.Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                Console.Clear();
                switch (loginChoice)
                {
                    case 1:
                        int c1 = NavigateMenu("Login");
                        if (c1 == 1)
                        {
                            System.Console.WriteLine("Login : Enter your user name or \"quit\" to exit");
                            try
                            {
                                userName = System.Console.ReadLine();
                                while (string.IsNullOrWhiteSpace(userName))
                                {
                                    if (string.IsNullOrWhiteSpace(userName))
                                    {
                                        System.Console.WriteLine("UserName cannot be null or empty , Please enter again");
                                        userName = System.Console.ReadLine();
                                    }
                                }
                                if (userName.Equals("quit", StringComparison.OrdinalIgnoreCase))
                                {
                                    break;
                                }
                                else
                                {
                                    System.Console.WriteLine("Login : Enter your user password");
                                    userPassword = PasswordBuilder.ReadPassword();
                                    while (string.IsNullOrWhiteSpace(userPassword))
                                    {
                                        System.Console.WriteLine("User Password cannot be null or empty , Please enter again");
                                        userPassword = PasswordBuilder.ReadPassword();
                                    }
                                    bool validatedUser = userService.ValidateUser(userName, userPassword);
                                    if (validatedUser)
                                    {
                                        System.Console.WriteLine("Login Successful");
                                        currentUserLoggedInRole = userService.GetUserRole(userName);
                                        currentUserLoggedInId = userService.GetUserId(userName);
                                        switch (currentUserLoggedInRole)
                                        {
                                            case "CUSTOMER":
                                                HandleCustomer();
                                                break;
                                            case "ADMIN":
                                                HandleAdmin();
                                                break;
                                            default:
                                                System.Console.WriteLine("invalid user role");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        System.Console.WriteLine("Login Unsuccessful , Try again");
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine(ex.Message);
                                Logger.LogException(ex);
                            }
                        }
                        else
                        {
                            break;
                        }
                        break;
                    case 2:
                        int c2 = NavigateMenu("Register new user");
                        if (c2 == 1)
                        {
                            System.Console.WriteLine("Registering new user");
                            try
                            {
                                userService.RegisterNewUser(userFactory, ref UserIdCounter);
                            }
                            catch(Exception ex)
                            {
                                System.Console.WriteLine(ex.Message);
                                Logger.LogException(ex);    
                            }
                        }
                        else
                        {
                            break;
                        }
                            break;
                    case 3:
                        Environment.Exit(1) ;
                        break;
                    default:System.Console.WriteLine("Enter valid choice input");
                        break;
                }
            }
            
        }
    }
}