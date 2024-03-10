using GameDatabase.Mongodb.Handlers;
using GameDatabase.Mongodb.Interfaces;
using GameOnlineServer.GameModels.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.GameModels.Handlers
{
    public class UserHandler : IDbHandler<User>
    {
        private readonly IGameDB<User> userDb;
        public UserHandler(IMongoDatabase database)
        {
            userDb = new MongoHandler<User>(database);
        }
        public User Create(User item)
        {
            return userDb.Create(item);
        }

        public User Find(string id)
        {
            var filter = Builders<User>.Filter.Eq(i => i.id,id);
            return userDb.Get(filter);
        }
        public User FindByUsername(string username)
        {
            var filter = Builders<User>.Filter.Eq(i => i.username, username);
            return userDb.Get(filter);
        }
        public List<User> FindAll()
        {
            return userDb.GetAll();
        }

        public void Remove(string id)
        {
            var filter = Builders<User>.Filter.Eq(i => i.id, id);
            userDb.Remove(filter);
        }

        public User Update(string id, User item)
        {
            var filter = Builders<User>.Filter.Eq(i => i.id, id);
            var updater = Builders<User>.Update
                .Set(i => i.password, item.password)
                .Set(i => i.avatar, item.avatar)
                .Set(i => i.amount, item.amount)
                .Set(i => i.updatedAt, DateTime.Now)
                .Set(i => i.level, item.level);
            return userDb.Update(filter, updater);
        }
    }
}
