using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class CategoryManager
    {
        private const string CategoryFilePath = "categories.txt"; 

        public static List<string> GetCategoriesFromFile()
        {
            if (!File.Exists(CategoryFilePath))
            {
                return new List<string>();
            }
            return new List<string>(File.ReadAllLines(CategoryFilePath));
        }

        public static void SaveCategoriesToFile(List<string> categories)
        {
            File.WriteAllLines(CategoryFilePath, categories);
        }   
    }
}
