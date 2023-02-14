using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Pet;
using Shuvi.Classes.Types.Cache;
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
                var pet = new
                _cache.TryAdd(data.Id, data);
            }
            catch (InvalidOperationException)
            {
                await AddUser(id);
            }
        } 
    }
}
