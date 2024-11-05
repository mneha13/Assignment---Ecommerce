using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private Dictionary<Type, object> _repositories;

        private DatabaseConnection()
        {
            _repositories = new Dictionary<Type, object>();
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnection();
                }
                return _instance;
            }
        }

        public Repository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<T>();
                _repositories[type] = repositoryInstance;
            }
            return (Repository<T>)_repositories[type];
        }
    }
}