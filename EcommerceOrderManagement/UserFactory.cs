using Ecommerce.Users.Admin;
using Ecommerce.Users.Customer;
using Ecommerce.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public  class UserFactory
    {
        public const string customer = "Customer";
        public const string admin = "Admin";
        public User GetUser(String role , String name , int id,String email,String password)
        {
            
            if (role.Equals(customer, StringComparison.OrdinalIgnoreCase))
            {
                return new Customer() {
                    UserId = id,
                    UserName = name,
                    UserEmail = email,
                    UserPassword = password,
                    UserRole = Role.CUSTOMER
                };   
            }

            if(role.Equals(admin, StringComparison.OrdinalIgnoreCase))
            {
                return new Customer()
                {
                    UserId = id,
                    UserName = name,
                    UserEmail = email,
                    UserPassword = password,
                    UserRole = Role.ADMIN
                };
            }
            return null;
        }
    }
}
