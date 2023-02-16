using MongoDB.Bson;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Items;

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
            throw new NotImplementedException();
        }
        public IItem GetItem(ObjectId id)
        {
            throw new NotImplementedException();
        }
        public int GetItemAmount(ObjectId id)
        {
            throw new NotImplementedException();
        }
        public IItem GetItemAt(int index)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<(TItem, int)> GetItems<TItem>() where TItem : IItem
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ObjectId> GetItemsId()
        {
            throw new NotImplementedException();
        }
    }
}
