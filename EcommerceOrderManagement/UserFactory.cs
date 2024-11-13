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
        public User? GetUser(String role , String name , int id,String email,String password)
        {
            var normalizedRole = role?.Trim().ToLower();
            return normalizedRole switch
            {
                StaticData.customer => CreateUser<Customer>(id, name, email, password, Role.CUSTOMER),
                StaticData.admin => CreateUser<Admin>(id, name, email, password, Role.ADMIN),
                _ => null,
            };
        }
        public T CreateUser<T>(int id, string name, string email, string password, Role role) where T : User, new()
        {
            return new T
            {
                UserId = id,
                UserName = name,
                UserEmail = email,
                UserPassword = password,
                UserRole = role
            };
        }
    }
}
