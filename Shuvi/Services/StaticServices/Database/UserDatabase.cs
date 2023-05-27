using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Factories.User;
using Shuvi.Classes.Types.Cache;
using Shuvi.Classes.Types.User;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.User;
using System.Linq.Expressions;

namespace Shuvi.Services.StaticServices.Database
{
    public static class UserDatabase
    {
        private static IMongoCollection<UserData>? _collection;
        private static readonly TemporaryCache<ulong, IDatabaseUser> _cache = new();

        public static void Init(IMongoCollection<UserData> collection)
        {
            _collection = collection;
        }
        public static async Task<IDatabaseUser> AddUser(ulong id)
        {
            var data = UserFactory.CreateUser(id);
            await _collection!.InsertOneAsync(data);
            var user = new DatabaseUser(data);
            _cache.TryAdd(user.Id, user);
            return user;
        }
        public static async Task<IDatabaseUser?> TryGetUser(ulong id)
        {
            IDatabaseUser? user;
            if (_cache.TryGet(id, out user))
                return user!;
            try
            {
                var data = await _collection.Find(new BsonDocument { { "_id", (long)id } }).SingleAsync();
                user = new DatabaseUser(data);
                _cache.TryAdd(user.Id, user);
                return user;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
        public static async Task<IDatabaseUser> GetUser(ulong id)
        {
            if (_cache.TryGet(id, out var user))
                return user!;
            try
            {
                var data = await _collection.Find(new BsonDocument { { "_id", (long)id } }).SingleAsync();
                user = new DatabaseUser(data);
                _cache.TryAdd(user.Id, user);
                return user;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"--- Пользователь {id} не найден. ---");
                throw;
            }
        }
        public static async Task UpdateUser(ulong id, UpdateDefinition<UserData> updateConfig)
        {
            await _collection!.UpdateOneAsync(new BsonDocument { { "_id", (long)id } }, updateConfig);
        }
        public static async Task<List<UserData>> GetTopUsers(Expression<Func<UserData, object>> sort, int limit)
        {
            return await _collection.Find(new BsonDocument())
                .SortByDescending(sort)
                .Limit(limit)
                .ToListAsync();
        }
        public static async Task<long> GetTotalUsers()
        {
            return await _collection.Find(new BsonDocument()).CountDocumentsAsync();
        }
    }
}
