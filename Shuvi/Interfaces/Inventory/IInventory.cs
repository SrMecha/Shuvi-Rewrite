using MongoDB.Bson;
using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Inventory
{
    public interface IInventory
    {
        public int Count { get; }

        public void AddItem(IItem item);
        public void AddItem(ObjectId id, int amount);
        public void RemoveItem(ObjectId id);
        public void RemoveItem(ObjectId id, int amount);
        public TItem GetItem<TItem>(ObjectId id) where TItem : IItem;
        public IItem GetItem(ObjectId id);
        public int GetItemAmount(ObjectId id);
        public IItem GetItemAt(int index);
        public IEnumerable<(TItem, int)> GetItems<TItem>() where TItem : IItem;
        public IEnumerable<ObjectId> GetItemsId();
    }
}
