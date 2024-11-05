using Ecommerce.PwdBuilder;
using Ecommerce.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ecommerce.Services.UserServices
{
    public class UserService
    {
        private readonly Repository<User> _userRepository;
        public const string quit = "quit";

        public UserService(Repository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public void Add(User user)
        {
            _userRepository.Add(user); 
        }

        //Checks the username and password during login to authenticate user
        public bool Validate( string name,string password)
        {
            IEnumerable<User> users = _userRepository.GetAll();
            var isPresent = _userRepository.Find(u => u.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (isPresent!=null && isPresent.UserName == name)
            {
                var userPassword = users
           .Where(user => user.UserName == name)
           .Select(user => user.UserPassword)
           .FirstOrDefault();
                if(userPassword.Equals(password, StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UserEmailExists(string email)
        {
            return _userRepository.Any(u => u.UserEmail.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public bool UserNameExists(string name)
        {
            return _userRepository.Any(u => u.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public string GetUserRole(string name)
        {
            var user = _userRepository.Find(u => u.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));
            string role = user.UserRole.ToString();
            return role;
        }

        public int GetUserId(string name)
        {
            var user = _userRepository.Find(u => u.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));
            int id = user.UserId;
            return id;
        }

        public User GetSingleUser(int id)
        {
            return _userRepository.Find(u => u.UserId == id);
        }
        public void ChangePassword(int id)
        {
            var user = _userRepository.Find(u => u.UserId == id);
            var oldPassword = user.UserPassword;
            string? newPassword = null;
            System.Console.WriteLine($"Enter new password for {user.UserName}");
            try
            {
                newPassword = PasswordBuilder.ReadPassword();
                while (string.IsNullOrWhiteSpace(newPassword) || newPassword==oldPassword)
                {
                    if (string.IsNullOrWhiteSpace(newPassword))
                    {
                        System.Console.WriteLine("Password cannot be null or empty , Please enter again");
                        newPassword = PasswordBuilder.ReadPassword();
                    }
                    else
                    {
                        System.Console.WriteLine("New password cannot be same as old password");
                        newPassword = PasswordBuilder.ReadPassword();
                    }
                }
                user.UserPassword = newPassword;
                System.Console.WriteLine("Password changed");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Logger.LogException(ex);
            }
        }

        public string PromptForNewUserName()
        {
            System.Console.WriteLine("Enter Username or \"quit\" to exit");
            string newUserName = System.Console.ReadLine();
            while (string.IsNullOrWhiteSpace(newUserName) || UserNameExists(newUserName))
            {
                if (UserNameExists(newUserName))
                {
                    System.Console.WriteLine("UserName already exits");
                    newUserName = System.Console.ReadLine();
                }
                else
                {
                    System.Console.WriteLine("UserName cannot be null or empty , Please enter again");
                    newUserName = System.Console.ReadLine();
                }
            }
            if (newUserName.Equals(quit, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
             return newUserName;
        }

        public string PromptForNewUserPassword()
        {
            System.Console.WriteLine("Enter Password");
            string newUserPassword = PasswordBuilder.ReadPassword();
            while (string.IsNullOrWhiteSpace(newUserPassword))
            {
                System.Console.WriteLine("Password cannot be null or empty , Please enter again");
                newUserPassword = PasswordBuilder.ReadPassword();
            }
            return newUserPassword;
        }
        public string PromptForNewUserRole()
        {
            System.Console.WriteLine("Enter User role : admin or customer or \"quit\" to exit");
            string newUserRole = System.Console.ReadLine();
            while (string.IsNullOrWhiteSpace(newUserRole) || !(newUserRole.Equals("Customer", StringComparison.OrdinalIgnoreCase) || newUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase) || newUserRole.Equals(quit, StringComparison.OrdinalIgnoreCase)))
            {
                System.Console.WriteLine(" Please enter valid user role");
                newUserRole = System.Console.ReadLine();
            }
            if (newUserRole.Equals(quit, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
             return newUserRole;
        }

        public string PromptForNewUserEmail()
        {
            System.Console.WriteLine("Enter User Email or \"quit\" to exit");
            string newUserEmail = System.Console.ReadLine();
            if (newUserEmail.Equals(quit, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            while (!newUserEmail.IsValidEmail() || UserEmailExists(newUserEmail))
            {
                if (UserEmailExists(newUserEmail))
                {
                    System.Console.WriteLine("Email already exits");
                    newUserEmail = System.Console.ReadLine();
                }
                else
                {
                     System.Console.WriteLine("Enter valid email");
                     newUserEmail = System.Console.ReadLine();
                }
            }
            return newUserEmail;
        }

        public void RegisterNewUser(UserFactory userFactory ,ref int UserIdCounter)
        {
            try
            {
                string newUserName = PromptForNewUserName();
                if (newUserName == null) return;
                string newUserPassword = PromptForNewUserPassword();
                System.Console.WriteLine("");
                string newUserRole = PromptForNewUserRole();
                if (newUserRole == null) return;
                string newUserEmail = PromptForNewUserEmail();
                if (newUserEmail == null) return;
                int newUserId = UserIdCounter++;
                User newUser = userFactory.GetUser(newUserRole, newUserName, newUserId, newUserEmail, newUserPassword);
                Add(newUser);
                System.Console.WriteLine("New user registeration completed");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Logger.LogException(ex);
            }
        }
    }
}
