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
        public const string quit = "quit";
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
            try
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
                int currentUserLoggedInId = 0;
                string userName, userPassword = "", currentUserLoggedInRole = Role.CUSTOMER.ToString();

                while (true)
                {
                    int loginChoice = 0;
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
                            if (c1 != 1) break;
                            UserLogin();
                            break;
                        case 2:
                            int c2 = NavigateMenu("Register new user");
                            if (c2 != 1) { break; }
                            System.Console.WriteLine("Registering new user");
                            userService.RegisterNewUser(userFactory, ref UserIdCounter);
                            break;
                        case 3:
                            Environment.Exit(1);
                            break;
                        default:
                            System.Console.WriteLine("Enter valid choice input");
                            break;
                    }
                }

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
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                        switch (choice)
                        {
                            case 1:
                                Console.Clear();
                                int c1 = NavigateMenu("Browse Products");
                                if(c1!= 1) { break; }
                                Console.Clear();
                                productService.DisplayAllProducts();
                                break;
                            case 2:
                                Console.Clear();
                                int c2 = NavigateMenu("Place Order");
                                if (c2 != 1) { break; }
                                Console.Clear();
                                Customer user = (Customer)userService.GetSingleUser(currentUserLoggedInId);
                                orderService.placeOrder(productService, currentUserLoggedInId, user);
                                Thread.Sleep(3000);
                                break;
                            case 3:
                                Console.Clear();
                                int c3 = NavigateMenu("Check Order status");
                                if (c3 != 1) { break; }
                                Console.Clear();
                                orderService.GetOrderStatusAsync(currentUserLoggedInId).GetAwaiter().GetResult();  
                                break;
                            case 4:
                                Console.Clear();
                                int c4 = NavigateMenu("Display products of a specific category");
                                if (c4 != 1) { break; }
                                Console.Clear();
                                productService.GetProductsByCategory();  
                                break;
                            case 5:
                                Console.Clear();
                                int c5 = NavigateMenu("View all orders");
                                if (c5 != 1) { break; }
                                Console.Clear();
                                orderService.DisplayOrdersByUserId(currentUserLoggedInId);    
                                break;
                            case 6:
                                Console.Clear();
                                int c6 = NavigateMenu("Cancel Order");
                                if (c6 != 1) { break; }
                                Console.Clear();
                                orderService.CancelOrder(currentUserLoggedInId, productService);    
                                break;
                            case 7:
                                Console.Clear();
                                int c7 = NavigateMenu("Change Password");
                                if (c7 != 1) { break; }
                                Console.Clear();
                                userService.ChangePassword(currentUserLoggedInId);   
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
                                if (c1 != 1) { break; }
                                Console.Clear();
                                System.Console.WriteLine("To quit at any point enter \"quit\" to exit");
                                productService.Add(ref productIdCounter, serviceProvider);   
                                break;
                            case 2:
                                Console.Clear();
                                int c2 = NavigateMenu("Remove Product");
                                if (c2 != 1) { break; }
                                Console.Clear();
                                productService.DeleteProduct();   
                                break;
                            case 3:
                                Console.Clear();
                                int c3 = NavigateMenu("View all orders");
                                if (c3 != 1) { break; }
                                Console.Clear();
                                orderService.DisplayAllOrders();
                                break;
                            case 4:
                                Console.Clear();
                                int c4 = NavigateMenu("View all orders by status");
                                if (c4 != 1) { break; }
                                Console.Clear();
                                orderService.DisplayAllOrdersByStatus();   
                                break;
                            case 5:
                                Console.Clear();
                                int c5 = NavigateMenu("View all orders by date");
                                if (c5 != 1) { break; }
                                Console.Clear();
                                orderService.DisplayAllOrdersByOrderDate();
                                break;
                            case 6:
                                Console.Clear();
                                int c6 = NavigateMenu("Display products of specific category");
                                if (c6 != 1) { break; }
                                Console.Clear();
                                productService.GetProductsByCategory();
                                break;
                            case 7:
                                Console.Clear();
                                int c7 = NavigateMenu("Update order status");
                                if (c7 != 1) { break; }
                                Console.Clear();
                                orderService.UpdateOrderStatus();
                                break;
                            case 8:
                                Console.Clear();
                                int c8 = NavigateMenu("View all products");
                                if (c8 != 1) { break; }
                                Console.Clear();
                                productService.DisplayAllProducts();
                                break;
                            case 9:
                                Console.Clear();
                                int c9 = NavigateMenu("Manage Inventory(add product quantity)");
                                if (c9 != 1) { break; }
                                Console.Clear();
                                productService.AddQuantity();
                                break;
                            case 10:
                                Console.Clear();
                                int c10 = NavigateMenu("Update product price");
                                if (c10 != 1) { break; }
                                Console.Clear();
                                productService.UpdatePrice();
                                break;
                            case 11:
                                Console.Clear();
                                int c11 = NavigateMenu("Change Password");
                                if (c11 != 1) { break; }
                                Console.Clear();
                                userService.ChangePassword(currentUserLoggedInId);
                                break;
                            case 0:
                                return;
                            default:
                                System.Console.WriteLine("Invalid choice");
                                break;
                        }
                    }
                }

                void HandleUserRole(string role)
                {
                    switch (role)
                    {
                        case "CUSTOMER":
                            HandleCustomer();
                            break;
                        case "ADMIN":
                            HandleAdmin();
                            break;
                        default:
                            System.Console.WriteLine("Invalid user role");
                            break;
                    }
                }

                string GetUserNameInput(string errorMessage, string quitOption)
                {
                    string input;
                    do
                    {
                        input = System.Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(input) && !input.Equals(quitOption, StringComparison.OrdinalIgnoreCase))
                        {
                            System.Console.WriteLine(errorMessage);
                        }
                    } while (string.IsNullOrWhiteSpace(input) && !input.Equals(quitOption, StringComparison.OrdinalIgnoreCase));

                    return input;
                }

                string GetPasswordInput(string errorMessage)
                {
                    string password;
                    do
                    {
                        password = PasswordBuilder.ReadPassword();
                        if (string.IsNullOrWhiteSpace(password))
                        {
                            System.Console.WriteLine(errorMessage);
                        }
                    } while (string.IsNullOrWhiteSpace(password));

                    return password;
                }
                void UserLogin()
                {
                    string userName, userPassword = "";
                    System.Console.WriteLine("Login : Enter your user name or \"quit\" to exit");
                    try
                    {
                        userName = GetUserNameInput("UserName cannot be null or empty, Please enter again", quit);
                        if (userName.Equals(quit, StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }
                        Console.WriteLine("Login: Enter your user password");
                        userPassword = GetPasswordInput("User Password cannot be null or empty, Please enter again");

                        if (!userService.Validate(userName, userPassword))
                        {
                            Console.WriteLine("Login Unsuccessful,Please Try again");
                            return;
                        }

                        Console.WriteLine("Login Successful");
                        currentUserLoggedInRole = userService.GetUserRole(userName);
                        currentUserLoggedInId = userService.GetUserId(userName);
                        HandleUserRole(currentUserLoggedInRole);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        Logger.LogException(ex);
                    }
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Logger.LogException(ex);
            }
        }
    }
}