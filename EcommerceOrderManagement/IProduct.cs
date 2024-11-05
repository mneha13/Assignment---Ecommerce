namespace Ecommerce.Products
{
        public interface IProduct
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public string ProductCategory {  get; set; }
            public long ProductPrice { get; set; }
            public int ProductQuantity { get; set; }
            public abstract void DisplayProductInfo(IProduct product);
        }
        public class Product : IProduct
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public string ProductCategory { get; set; }
            public long ProductPrice { get; set; }
            public int ProductQuantity { get; set; }

            public void DisplayProductInfo(IProduct product)
            {
                System.Console.WriteLine($""" 
                        **Product Information** 
                    Id = {product.ProductID}
                    Name = {product.ProductName}
                    Desc = {product.ProductDescription}
                    Category = {ProductCategory}
                    Price = {product.ProductPrice}
                    Qty = {product.ProductQuantity}
                    """);
            }
        }
}
