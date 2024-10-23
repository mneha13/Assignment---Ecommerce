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
        public User GetUser(String role , String name , int id,String email,String password)
        {
            if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return new Customer() {
                    UserId = id,
                    UserName = name,
                    UserEmail = email,
                    UserPassword = password,
                    UserRole = Role.CUSTOMER
                };   
            }

            else if(role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
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
