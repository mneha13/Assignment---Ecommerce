using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ecommerce
{
    public class Repository<T> where T : class
    {
        private readonly List<T> _dataStore = new List<T>();
        private readonly string _filePath;
        public Repository(string filePath)
        {
            _filePath = filePath;
            _dataStore = LoadDataFromFile();
        }
        public void SaveDataToFile()
        {
            try
            {
                // Use TypeNameHandling to include the type information in the JSON
                string json = JsonConvert.SerializeObject(_dataStore, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data to file: {ex.Message}");
            }
        }

        private List<T> LoadDataFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    return JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All // Ensure the type info is used when deserializing
                    }) ?? new List<T>();
                }
                else
                {
                    return new List<T>(); // Return an empty list if the file doesn't exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from file: {ex.Message}");
                return new List<T>(); // Return an empty list in case of error
            }
        }

        public void Add(T item)
        {
            _dataStore.Add(item);
            SaveDataToFile();
        }

        public void Remove(T item)
        {
            _dataStore.Remove(item);
            SaveDataToFile();
        }

        public T Find(Func<T, bool> predicate)
        {
            return _dataStore.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _dataStore;
        }

        public int CountItems()
        {
            return (_dataStore.Count);
        }
        public bool Any(Func<T, bool> predicate)
        {
            return _dataStore.Any(predicate);
        }
    }
}
