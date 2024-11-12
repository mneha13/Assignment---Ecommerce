namespace Ecommerce
{
    public class CategoryManager
    {
        public static List<string> GetCategoriesFromFile()
        {
            try
            {
                if (!File.Exists(StaticData.CategoryFilePath))
                {
                    return new List<string>();
                }
                else
                {
                    return new List<string>(File.ReadAllLines(StaticData.CategoryFilePath));
                }
            }
            catch(IOException ex)
            {
                System.Console.WriteLine(ex.Message);
                return new List<string>();
            }
        }

        public static void SaveCategoriesToFile(List<string> categories)
        {
            try
            {
                File.WriteAllLines(StaticData.CategoryFilePath, categories);
            }
            catch (IOException ex)
            {
                System.Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }   
    }
}
