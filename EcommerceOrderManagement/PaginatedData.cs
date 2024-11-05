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
        public static void Paginate<T>(List<T> items, int pageSize)
        {
            int totalPages = (int)Math.Ceiling((double)items.Count / pageSize);
            int currentPage = 1;

            while (true)
            {
                Console.Clear();
                System.Console.WriteLine($"Page {currentPage}/{totalPages}\n");
                DisplayPage(items, currentPage, pageSize);
                System.Console.WriteLine("\nNavigation: (N)ext, (P)revious, (E)xit");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.E)
                {
                    break;
                }
                if (key == ConsoleKey.N && currentPage < totalPages)
                {
                    currentPage++;
                }
                if (key == ConsoleKey.P && currentPage > 1)
                {
                    currentPage--;
                }
            }
        }

        public static void DisplayPage<T>(List<T> items, int currentPage, int pageSize)
        {
            int start = (currentPage - 1) * pageSize;
            int end = Math.Min(start + pageSize, items.Count);
            for (int index = start; index < end; index++)
            {
                switch (items[index])
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