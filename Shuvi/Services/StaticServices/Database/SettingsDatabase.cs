using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Shuvi.Classes.Data.Map;
using Shuvi.Classes.Data.Settings;


namespace Shuvi.Services.StaticServices.Database
{
    public sealed class SettingsDatabase
    {
        private static IMongoCollection<BsonDocument>? _collection;

        public static void Init(IMongoCollection<BsonDocument> collection)
        {
            _collection = collection;
        }
        public static async Task<LogsData> LoadLogsData()
        {
            return BsonSerializer.Deserialize<LogsData>(await _collection.Find(new BsonDocument { { "_id", "Logs" } }).SingleAsync());
        }
        public static AdminsData LoadAdminsData()
        {
            return BsonSerializer.Deserialize<AdminsData>(_collection.Find(new BsonDocument { { "_id", "Admins" } }).Single());
        }
        public static async Task UpdateAdmins(UpdateDefinition<BsonDocument> updateConfig)
        {
            await _collection!.UpdateOneAsync(new BsonDocument { { "_id", "Admins" } }, updateConfig);
        }
        public static BotInfoData LoadBotInfoData()
        {
            return BsonSerializer.Deserialize<BotInfoData>(_collection.Find(new BsonDocument { { "_id", "Info" } }).Single());
        }
        public static async Task UpdateBotInfo(UpdateDefinition<BsonDocument> updateConfig)
        {
            await _collection!.UpdateOneAsync(new BsonDocument { { "_id", "Info" } }, updateConfig);
        }
        public static WorldMapData LoadMap()
        {
            return BsonSerializer.Deserialize<WorldMapData>(_collection.Find(new BsonDocument { { "_id", "Map" } }).Single());
        }
    }
}
