using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Products;

namespace Ecommerce.Users.Admin
    {
        public class Admin : User
        {
            public override void DisplayUserInfo()
            {
                System.Console.WriteLine($"Admin Details : ID - {UserId} Name - {UserName} Role - {UserRole} ");
            }

            public void ManageInventory(IProduct Product, int Qty)
            {
                Product.ProductQuantity = Qty;
                System.Console.WriteLine($"Quantity for {Product.ProductName} is updated");
            }
        }
    }

