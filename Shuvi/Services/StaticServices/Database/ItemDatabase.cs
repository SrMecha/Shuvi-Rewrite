using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Factories.Item;
using Shuvi.Classes.Types.Item;
using Shuvi.Interfaces.Items;

namespace Shuvi.Services.StaticServices.Database
{
    public static class ItemDatabase
    {
        private static IMongoCollection<ItemData>? _collection;
        private static Dictionary<ObjectId, IItem> _cache = new();

        public static List<IItem> Items { get; private set; } = new();

        public static void Init(IMongoCollection<ItemData> collection)
        {
            _collection = collection;
            LoadAllItems();
        }
        private static void LoadAllItems()
        {
            foreach (var data in _collection.FindSync(new BsonDocument { }).ToEnumerable<ItemData>())
            {
                Items.Add(ItemFactory.CreateItem(data));
                _cache.Add(data.Id, ItemFactory.CreateItem(data));
            }
        }
        public static IItem GetItem(ObjectId id)
        {
            return _cache.GetValueOrDefault(id, new SimpleItem(new()));
        }
        public static TItem GetItem<TItem>(ObjectId id) where TItem : IItem
        {
            return (TItem)_cache.GetValueOrDefault(id, new SimpleItem(new()));
        }
    }
}
