using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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
        public static LogsData LoadLogsData()
        {
            return BsonSerializer.Deserialize<LogsData>(_collection.Find(new BsonDocument { { "_id", "Logs" } }).Single());
        }
        public static AdminsData LoadAdminsData()
        {
            return BsonSerializer.Deserialize<AdminsData>(_collection.Find(new BsonDocument { { "_id", "Admins" } }).Single());
        }
        public static async Task UpdateAdmins(UpdateDefinition<AdminsData> updateConfig)
        {
            await _collection!.UpdateOneAsync(new BsonDocument { { "_id", "Admins" } }, updateConfig.ToBsonDocument());
        }
        public static BotInfoData LoadBotInfoData()
        {
            return BsonSerializer.Deserialize<BotInfoData>(_collection.Find(new BsonDocument { { "_id", "Info" } }).Single());
        }
        public static async Task UpdateBotInfo(UpdateDefinition<BotInfoData> updateConfig)
        {
            await _collection!.UpdateOneAsync(new BsonDocument { { "_id", "Info" } }, updateConfig.ToBsonDocument());
        }
    }
}
