using MongoDB.Driver;

namespace GameDatabase.Mongodb.Handlers
{
    public class MongoDb
    {
        private readonly IMongoClient client;
        private IMongoDatabase database => client.GetDatabase("GameOnline");
        public MongoDb()
        {
            var setting = MongoClientSettings.FromConnectionString("mongodb://localhost:27017/");
            client = new MongoClient(setting);
        }

        public IMongoDatabase GetDataBase()
        {
            return database;
        }
    }
}
