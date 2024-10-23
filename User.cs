namespace Ecommerce.Users
    {
        public abstract class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string UserPassword { get; set; }
            public string UserEmail { get; set; }
            public Role UserRole { get; set; }

        public abstract void DisplayUserInfo();
    }
 }
