using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Enemy;
using Shuvi.Classes.Types.Enemy;
using Shuvi.Interfaces.Enemy;

namespace Shuvi.Services.StaticServices.Database
{
    public static class EnemyDatabase
    {
        private static IMongoCollection<EnemyData>? _collection;
        private static Dictionary<ObjectId, IDatabaseEnemy> _cache = new();

        public static void Init(IMongoCollection<EnemyData> collection)
        {
            _collection = collection;
            LoadAllEnemies();
        }
        private static void LoadAllEnemies()
        {
            foreach (var data in _collection.FindSync(new BsonDocument { }).ToEnumerable<EnemyData>())
            {
                _cache.Add(data.Id, new DatabaseEnemy(data));
            }
        }
        public static IDatabaseEnemy GetEnemy(ObjectId id)
        {
            return _cache.GetValueOrDefault(id, new DatabaseEnemy(new()));
        }
    }
}
