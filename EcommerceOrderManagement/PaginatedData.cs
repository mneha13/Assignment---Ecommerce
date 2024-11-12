using Ecommerce.Products;
using Ecommerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class PaginatedData
    {
        public static void Paginate<T>(List<T> items)
        {
            int pageSize = StaticData.defaultPageSize;
            if (pageSize <= 0)
            {
                Console.WriteLine("Page size must be greater than zero.");
                return;
            }
            int totalPages = (int)Math.Ceiling((double)items.Count / pageSize);
            int currentPage = 1;

            while (true)
            {
                Console.Clear();
                System.Console.WriteLine($"Page {currentPage}/{totalPages}\n");
                DisplayPage(items, currentPage, pageSize);
                System.Console.WriteLine("\nNavigation: (N)ext, (P)revious, (E)xit");
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.E:
                        return;
                    case ConsoleKey.N when currentPage < totalPages:
                        currentPage++;
                        break;
                    case ConsoleKey.P when currentPage > 1:
                        currentPage--;
                        break;
                }
            }
        }

        public static void DisplayPage<T>(List<T> items, int currentPage, int pageSize)
        {
            int start = (currentPage - 1) * pageSize;
            int end = Math.Min(start + pageSize, items.Count);
            if (start >= items.Count || currentPage <= 0 || pageSize <= 0)
            {
                Console.WriteLine("Invalid page or page size.");
                return;
            }
            for (int i = start; i < end; i++)
            {
                switch (items[i])
                {
                    case Product product:
                        product.DisplayProductInfo(product);
                        break;
                    case Order order:
                        order.DisplayOrderInfo(order);
                        break;
                }
            }
        }
    }
}