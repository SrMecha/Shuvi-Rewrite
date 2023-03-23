using MongoDB.Bson;
using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Inventory
{
    public interface IReadOnlyInventory
    {
        public int Count { get; }

        public bool HaveItem(ObjectId id, int amount = 1);
        public TItem GetItem<TItem>(ObjectId id) where TItem : IItem;
        public IItem GetItem(ObjectId id);
        public int GetItemAmount(ObjectId id);
        public int GetItemAmountAt(int index);
        public IItem GetItemAt(int index);
        public IEnumerable<(TItem, int)> GetItems<TItem>() where TItem : IItem;
        public IEnumerable<(IItem, int)> GetItems();
        public IEnumerable<ObjectId> GetItemsId();
        public Dictionary<ObjectId, int> GetItemsDictionary();
        public Dictionary<string, int> GetDictionaryToSave();
    }
}
