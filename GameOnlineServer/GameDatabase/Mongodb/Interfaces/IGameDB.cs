using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.Mongodb.Interfaces
{
    public interface IGameDB<T> where T : class
    {
        IMongoDatabase GetDataBase();
        IMongoCollection<T> GetCollection(string name);
        T Get(FilterDefinition<T> filter);
        List<T> GetAll();
        T Create(T item);
        void Remove(FilterDefinition<T> filter);
        T Update(FilterDefinition<T> filter, UpdateDefinition<T> updadter);
    }
}
