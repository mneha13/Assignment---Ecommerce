using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Products;

namespace Ecommerce.Users.Customer
    {
        public class Customer : User
        {
        public List<Address> addresses = new List<Address>();
        
        public override void DisplayUserInfo()
            {
                System.Console.WriteLine($"Customer Details : ID - {UserId} Name - {UserName} Role - {UserRole} ");
            }

            public void PlaceOrder(IProduct product)
            {
                System.Console.WriteLine($"Successful order placed for product {product.ProductName} ");
            }
        }
    }

