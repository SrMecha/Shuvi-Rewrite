using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Pet;
using Shuvi.Classes.Types.Cache;
using Shuvi.Classes.Types.Pet;
using Shuvi.Interfaces.Pet;

namespace Shuvi.Services.StaticServices.Database
{
    public static class PetDatabase
    {
        private static IMongoCollection<PetData>? _collection;
        private static readonly TemporaryCache<ObjectId, IDatabasePet> _cache = new();

        public static void Init(IMongoCollection<PetData> collection)
        {
            _collection = collection;
        }
        public static async Task<IDatabasePet?> GetPet(ObjectId id)
        {
            if (_cache.TryGet(id, out var pet))
                return pet!;
            try
            {
                var data = await _collection.Find(new BsonDocument { { "_id", id } }).SingleAsync();
                pet = new DatabasePet(data);
                _cache.TryAdd(data.Id, pet);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return null;
        }
        public static async Task UpdatePet(ObjectId id, UpdateDefinition<PetData> updateConfig)
        {
            await _collection!.UpdateOneAsync(new BsonDocument { { "_id", id } }, updateConfig);
        }
    }
}
