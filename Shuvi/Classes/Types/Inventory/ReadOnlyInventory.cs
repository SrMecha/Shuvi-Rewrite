using MongoDB.Bson;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Items;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Inventory
{
    public class ReadOnlyInventory : IReadOnlyInventory
    {
        protected Dictionary<ObjectId, int> _items = new();

        public int Count { get { return _items.Count; } }

        public ReadOnlyInventory(Dictionary<ObjectId, int> items)
        {
            _items = items;
        }
        public TItem GetItem<TItem>(ObjectId id) where TItem : IItem
        {
            return ItemDatabase.GetItem<TItem>(id);
        }
        public IItem GetItem(ObjectId id)
        {
            return ItemDatabase.GetItem(id);
        }
        public int GetItemAmount(ObjectId id)
        {
            return _items.GetValueOrDefault(id, 0);
        }
        public int GetItemAmountAt(int index)
        {
            return _items.ElementAt(index).Value;
        }
        public IItem GetItemAt(int index)
        {
            return ItemDatabase.GetItem(_items.ElementAt(index).Key);
        }
        public IEnumerable<(TItem, int)> GetItems<TItem>() where TItem : IItem
        {
            foreach (var (id, amount) in _items)
                yield return (GetItem<TItem>(id), amount);
        }
        public IEnumerable<(IItem, int)> GetItems()
        {
            foreach (var (id, amount) in _items)
                yield return (GetItem(id), amount);
        }
        public IEnumerable<ObjectId> GetItemsId()
        {
            return _items.Keys;
        }
    }
}
