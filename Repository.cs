using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class Repository<T> where T : class
    {
        private readonly List<T> _dataStore = new List<T>();

        public void Add(T item)
        {
            _dataStore.Add(item);
        }

        public void Remove(T item)
        {
            _dataStore.Remove(item);
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
    }
}
