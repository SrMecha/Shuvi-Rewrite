using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Item;
using Shuvi.Interfaces.Items;

namespace Shuvi.Services.StaticServices.Database
{
    public static class ItemDatabase
    {
        private static IMongoCollection<ItemData>? _collection;
        private static Dictionary<ObjectId, IItem> _cache = new();

        public static void Init(IMongoCollection<ItemData> itemCollection)
        {
            _collection = itemCollection;
            LoadAllItems();
        }

        private void LoadAllItems()
        {
            foreach (var data in _collection.FindSync(new BsonDocument { }).ToEnumerable<ItemData>())
            {
                _cache.Add(data.Id, );
            }
        }
    }
}
