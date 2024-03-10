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
    public class RoomHandler : IDbHandler<RoomModel>
    {
        private IGameDB<RoomModel> roomDb;
        public RoomHandler(IMongoDatabase database) { 
            roomDb = new MongoHandler<RoomModel>(database);
        }
        public RoomModel Create(RoomModel item)
        {
            return roomDb.Create(item);
        }

        public RoomModel Find(string id)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.id, id);
            return roomDb.Get(filter);
        }

        public List<RoomModel> FindAll()
        {
            return roomDb.GetAll();
        }

        public void Remove(string id)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.id, id);
            roomDb.Remove(filter);
        }

        public RoomModel Update(string id, RoomModel item)
        {
            var filter = Builders<RoomModel>.Filter.Eq(i => i.id, id);
            var updater = Builders<RoomModel>.Update
                .Set(i => i.roomName, item.roomName);
            return roomDb.Update(filter, updater);
        }
    }
}
