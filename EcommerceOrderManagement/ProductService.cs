using Ecommerce.Products;
using Ecommerce.Services.OrderServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.ProductServices
{
    public class ProductService
    {
        private readonly Repository<Product> _productRepository;
        public static List<string> ProductCategories = new List<string> { "Electronics", "Food", "Health", "Sports", "Fashion", "Games", "Books", "Office Supplies" };
        public const string quit = "quit";
        
        public ProductService(Repository<Product> productRepository, Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider)
        {
            _productRepository = productRepository;
        }

        public void Add(ref int productIdCounter, ServiceProvider serviceProvider)
        {
            var product = serviceProvider.GetRequiredService<Product>();
            product = GetValidProduct(ref productIdCounter, product);
            if (product != null)
            {
                _productRepository.Add(product);
                System.Console.WriteLine("Product added successfully");
            }
        }
        public Product GetValidProduct(ref int productIdCounter,Product product)
        {
            string prodName, prodDesc, prodCat;
            prodName = GetProductName();
            if (string.IsNullOrWhiteSpace(prodName))
            {
                return null;
            }
            int prodPrice = GetProductPrice();
            if (prodPrice == GetQuitValue())
            {
                return null;
            }
            prodDesc = GetProductDesc();
            if (string.IsNullOrWhiteSpace(prodDesc))
            {
                return null;
            }
            System.Console.WriteLine("Select category choice");
            prodCat = GetProductCategories();
            if (prodCat == quit)
            {
                return null;
            }
            int prodQty = GetProductQty();
            if (prodQty == GetQuitValue())
            {
                return null;
            }
            int prodId = productIdCounter++;
            product.ProductID = prodId; ;
            product.ProductName = prodName;
            product.ProductPrice = prodPrice;
            product.ProductDescription = prodDesc;
            product.ProductCategory = prodCat;
            product.ProductQuantity = prodQty;
            return product; 
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }
        public bool HasProduct(string name)
        {
            return _productRepository.Any(p => p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public int IsValidUserChoice(int startRange , int endRange)
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

        public void DeleteProduct()
        {
            var products = _productRepository.GetAll().ToList();
            if(products.Count == 0)
            {
                System.Console.WriteLine("No Products present to be deleted");
                return;
            }
            int index = 0;
            foreach (var item in products)
            {
               System.Console.WriteLine($"{index} : {item.ProductName}");
               index++;
            }
            System.Console.WriteLine($"{products.Count()} : To Go Back (Exit)");
            int choice = IsValidUserChoice(0, products.Count());
            string name;
            if(choice == products.Count())
            {
                return;//if user chooses to go back
            }
            Product productToDelete = _productRepository.Find(p => p.ProductID == products[choice].ProductID);
            if (productToDelete != null)
            {
               name = productToDelete.ProductName;
               _productRepository.Remove(productToDelete);
               System.Console.WriteLine($"{name} Product deleted");
            }        
        }

        public int GetQuantity(string name)
        {
            Product prod = _productRepository.Find(p => p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return prod.ProductQuantity;
        }

        public void SetQuantity(string name,int qty)
        {
            Product prod = _productRepository.Find(p => p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
            prod.ProductQuantity = prod.ProductQuantity - qty;
        }

        public void AddQuantity()
        {
            try
            {
                if (_productRepository.GetAll().Count() == 0)
                {
                    System.Console.WriteLine("There are no products in inventory");
                    return;
                }
                var products = _productRepository.GetAll().ToList();
                int index = 0;
                foreach(var product in products)
                {
                   System.Console.WriteLine($"{index} : {product.ProductName} , quantity in stock {product.ProductQuantity}");
                   index++;
                }
                System.Console.WriteLine($"{products.Count} : To Go Back (Exit)");
                int choice = IsValidUserChoice(0, products.Count());
                if (choice == products.Count())
                {
                    return;  // if user chooses to exit
                }
                String name = products[choice].ProductName;
                Product prod = _productRepository.Find(p => p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (prod != null)
                {
                     String quantity = "";
                     bool isValidInput = false;
                     while (!isValidInput)
                     {
                         System.Console.WriteLine("Enter quantity to be added");
                        quantity = System.Console.ReadLine();
                         isValidInput = int.TryParse(quantity, out _);
                         if (!isValidInput)
                         {
                              System.Console.WriteLine("Invalid quantity. Please try again.");
                         }
                     }
                     prod.ProductQuantity += System.Convert.ToInt32(quantity);
                     if (System.Convert.ToInt32(quantity) > 0)
                     {
                          System.Console.WriteLine("Quantity updated");
                     }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Logger.LogException(ex);
            }
        }

        public void UpdatePrice()
        {
            try
            {
                if (_productRepository.GetAll().Count() == 0)
                {
                    System.Console.WriteLine("There are no products in inventory");
                    return;
                }
                var products = _productRepository.GetAll().ToList();
                int index = 0;
                foreach (var product in products)
                {
                    System.Console.WriteLine($"{index} : {product.ProductName} originally priced at {product.ProductPrice}");
                    index++;
                }
                System.Console.WriteLine($"{products.Count} : To Go Back (Exit)");
                int choice = IsValidUserChoice(0,products.Count());
                if (choice == products.Count()) { return; }
                String name = products[choice].ProductName;
                Product prod = _productRepository.Find(p => p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (prod != null)
                {
                    String price = "";
                    bool isValidInput = false;
                    while (!isValidInput)
                    {
                        System.Console.WriteLine("Enter price to be updated");
                        price = System.Console.ReadLine();
                        isValidInput = int.TryParse(price, out _);
                        if (!isValidInput)
                        {
                            System.Console.WriteLine("Invalid price input. Please try again.");
                        }
                    }
                    prod.ProductPrice = System.Convert.ToInt32(price);
                    System.Console.WriteLine($"Price updated for {prod.ProductName}");
                }
             }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Logger.LogException(ex);
            }
        }

        public void GetProductsByCategory()
        {
            var products = _productRepository.GetAll();
            if (products.Count() == 0)
            {
                System.Console.WriteLine("There are no products to show them category wise");
                return;
            }
            var uniqueCategories = products.Select(p => p.ProductCategory).Distinct().ToList();
            int index = 0;
            foreach (var category in uniqueCategories)
            {
                System.Console.WriteLine($"{index} : {category}");
                index++;
            }
            System.Console.WriteLine($"{uniqueCategories.Count()} : To Go Back (Exit)");
            int choice = IsValidUserChoice(0,uniqueCategories.Count());
            if (choice == uniqueCategories.Count()) { return;  }
            string prodCategory = uniqueCategories[choice];
            IEnumerable<Product> filteredProducts = new List<Product>();
            filteredProducts = _productRepository.GetAll().Where(p => p.ProductCategory.Contains(prodCategory));
            if (filteredProducts.Count() == 0)
            {
                System.Console.WriteLine("There are no products in this category");
                return;
            }
             PaginatedData.Paginate(filteredProducts.ToList(), 3);                
        }

        public void DisplayAllProducts()
        {
            var products = _productRepository.GetAll();
            if (products.Count() == 0)
            {
                System.Console.WriteLine("There are no products to display");
                return;
            }
            System.Console.WriteLine($"""
                1 : View by category
                2 : View by price - Lowest first
                3 : View by price - Highest first
                4 : View all
                5 : Exit
            """);
            int choice = IsValidUserChoice(0, 5);
            switch (choice)
            {
                case 1:
                    var categorizedProducts = products.GroupBy(p => p.ProductCategory);
                    var allcategoryproducts = new List<Product>();
                    foreach (var group in categorizedProducts)
                    {
                        var allProducts = group.ToList();
                        allcategoryproducts.AddRange(allProducts);
                    }
                    PaginatedData.Paginate(allcategoryproducts, 2);
                    break;
                case 2:
                    var sortedProducts = products.OrderBy(p => p.ProductPrice);
                    PaginatedData.Paginate(sortedProducts.ToList(), 2);
                    break;
                case 3:
                    var sortedDescProducts = products.OrderByDescending(p => p.ProductPrice);
                    PaginatedData.Paginate(sortedDescProducts.ToList(), 2);
                    break;
                case 4:
                    PaginatedData.Paginate(products.ToList(), 2);
                    break;
                case 5: return;
            }
        }

        public string GetProductNameById(int id)
        {
            Product product = _productRepository.Find(p=>p.ProductID == id);
            if(product != null)
            {
                return product.ProductName;
            }
            return null;
        }

        public int GetOrderAmount(int qty , string name)
        {
            Product prod = _productRepository.Find(p => p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
            int total = (int)(prod.ProductPrice * qty);
            return total;
        }

        //To update quantity of product if user cancels order
        public void UpdateProductQty(int qty , string name)
        {
            Product prod = _productRepository.Find(p => p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
            prod.ProductQuantity += qty;
        }


        public  string GetProductName()
        {
            String prodName="";
            System.Console.WriteLine("Enter product name: ");
            prodName = System.Console.ReadLine();
            while (string.IsNullOrWhiteSpace(prodName) || HasProduct(prodName))
            {
                if (string.IsNullOrWhiteSpace(prodName))
                {
                    System.Console.WriteLine("Product name cannot be null or empty , Please enter again");
                    prodName = System.Console.ReadLine();
                }
                else
                {
                    System.Console.WriteLine("This product already exists , Please enter again");
                    prodName = System.Console.ReadLine();
                }
            }
            if (prodName.Equals(quit, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            return prodName;
        }
        public int GetProductPrice()
        {
                String price = "";
                bool isValidInput = false;
                System.Console.WriteLine("Enter product price: ");
                while (!isValidInput)
                {
                    price = System.Console.ReadLine();
                    if (!price.Equals(quit, StringComparison.OrdinalIgnoreCase))
                    {
                        isValidInput = int.TryParse(price, out _);
                        if (!isValidInput)
                        {
                            Console.WriteLine("Invalid price input. Please try again.");
                        }
                    }
                    if (price.Equals(quit, StringComparison.OrdinalIgnoreCase))
                    {
                        return GetQuitValue();
                    }
                }
                return System.Convert.ToInt32(price);
        }
        public  string GetProductDesc()
        {
            string prodDesc = "";
            System.Console.WriteLine("Enter product description: ");
            prodDesc = System.Console.ReadLine();
            while (string.IsNullOrWhiteSpace(prodDesc))
            {
                System.Console.WriteLine("Product description cannot be null or empty , Please enter again");
                prodDesc = System.Console.ReadLine();
            }
            if (prodDesc.Equals(quit, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
             return prodDesc;
        }
        public  int GetProductQty()
        {
            String quantity = "";
            bool isValidQty = false;
            while (!isValidQty)
            {
                System.Console.WriteLine("Enter product quantity: ");
                quantity = System.Console.ReadLine();
                if (!quantity.Equals(quit, StringComparison.OrdinalIgnoreCase))
                {
                    isValidQty = int.TryParse(quantity, out _);
                    if (!isValidQty)
                    {
                        Console.WriteLine("Invalid quantity input. Please try again.");
                    }
                }
                if (quantity.Equals(quit, StringComparison.OrdinalIgnoreCase))
                {
                    return GetQuitValue();
                }
            }
            return System.Convert.ToInt32(quantity);
        }

        public string GetProductCategories()
        {
            int count = ProductCategories.Count;
            for (int index = 0; index < count; index++)
            {
                System.Console.WriteLine($"{index} : {ProductCategories[index]}");
            }
            System.Console.WriteLine($"{count} : Add new category");

            while (true)
            {
                System.Console.WriteLine("Enter choice or type 'quit' to exit:");
                string userChoice = System.Console.ReadLine();

                if (userChoice.Equals(quit, StringComparison.OrdinalIgnoreCase))
                {
                    return quit;
                }

                if (int.TryParse(userChoice, out int choice) && choice >= 0 && choice <= count)
                {
                    return choice < count ? ProductCategories[choice] : AddNewCategory();
                }

                Console.WriteLine("Invalid choice input. Please try again.");
            }
        }

        private string AddNewCategory()
        {
            while (true)
            {
                System.Console.WriteLine("Enter category name:");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    System.Console.WriteLine("Category name cannot be null or empty, please enter again.");
                    continue;
                }

                if (ProductCategories.Any(s => s.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    System.Console.WriteLine("Category name already exists, enter a new category or type 'quit' to exit:");
                    if (System.Console.ReadLine().Equals(quit, StringComparison.OrdinalIgnoreCase))
                    {
                        return quit;
                    }
                    continue;
                }

                ProductCategories.Add(name);
                return name;
            }
        }
        public int GetQuitValue()
        {
            return -1;
        }
    }
}
