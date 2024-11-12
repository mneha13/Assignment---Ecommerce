using Ecommerce.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public static class StaticData
    {
        public const string CategoryFilePath = "categories.txt";
        public static readonly string logFilePath = "app.log";
        public static int orderIdCounter = 1;
        public static int defaultPageSize = 3;
        public const string quit = "quit";
        public const int quitValue = -1;
        public static int productIdCounter = 1, UserIdCounter = 1;
        public const string customer = "customer";
        public const string admin = "admin";
        public static int IsValidUserChoice(int startRange, int endRange)
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
    }
}
