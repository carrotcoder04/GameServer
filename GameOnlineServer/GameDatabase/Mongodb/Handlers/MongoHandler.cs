using Amazon.Runtime.Internal.Settings;
using GameDatabase.Mongodb.Interfaces;
using MongoDB.Driver;
using System.Collections;

namespace GameDatabase.Mongodb.Handlers
{
    public class MongoHandler<T> : IGameDB<T> where T : class
    {
        private IMongoDatabase database;
        private IMongoCollection<T> collection;
        public MongoHandler(IMongoDatabase database)
        {
            this.database = database;
            this.SetCollection();
        }
        private void SetCollection()
        {
            switch(typeof(T).Name)
            {
                case "User":
                    
                    collection = database.GetCollection<T>("Users");
                    break;
                case "Room":
                    collection = database.GetCollection<T>("Rooms");
                    break;
                default:
                    break;
            }
        }
        public T Create(T item)
        {
            collection.InsertOne(item);
            return item;
        }

        public List<T> GetAll()
        {
            var filter = Builders<T>.Filter.Empty;
            return collection.Find(filter).ToList();
        }
        public IMongoCollection<T> GetCollection(string colName)
        {
            return database.GetCollection<T>(colName);
        }
        public IMongoDatabase GetDataBase()
        {
            return database;
        }

        public T Get(FilterDefinition<T> filter)
        {
            return collection.Find(filter).FirstOrDefault();
        }

        public void Remove(FilterDefinition<T> filter)
        {
            collection.DeleteOne(filter);
        }

        public T Update(FilterDefinition<T> filter, UpdateDefinition<T> updadter)
        {
            collection.UpdateOne(filter, updadter);
            return Get(filter);
        }
    }
}
