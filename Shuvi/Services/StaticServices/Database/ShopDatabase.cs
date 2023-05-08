using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Shop;
using Shuvi.Classes.Types.Shop;
using Shuvi.Interfaces.Shop;

namespace Shuvi.Services.StaticServices.Database
{
    public static class ShopDatabase
    {
        private static IMongoCollection<ShopData>? _collection;
        private static readonly Dictionary<ObjectId, IReadonlyShop> _cache = new();

        public static void Init(IMongoCollection<ShopData> collection)
        {
            _collection = collection;
            LoadShops();
        }

        private static void LoadShops()
        {
            foreach (var data in _collection.FindSync(new BsonDocument { }).ToEnumerable<ShopData>())
            {
                _cache.Add(data.Id, new ReadonlyShop(data));
            }
        }

        public static IShop CreateShop(ObjectId id)
        {
            if (_cache.TryGetValue(id, out var shop))
                return shop.CreateShop();
            throw new NotImplementedException("ID магазина не найдено.");
        }

        public static IReadonlyShop GetReadonlyShop(ObjectId id)
        {
            if (_cache.TryGetValue(id, out var shop))
                return shop;
            throw new NotImplementedException("ID магазина не найдено.");
        }
    }
}
