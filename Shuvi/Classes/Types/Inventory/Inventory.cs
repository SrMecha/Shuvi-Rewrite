using MongoDB.Bson;
using Shuvi.Interfaces.Inventory;

namespace Shuvi.Classes.Types.Inventory
{
    public class Inventory : ReadOnlyInventory, IInventory
    {
        public Inventory(Dictionary<ObjectId, int> items) : base(items)
        {

        }
        public void AddItem(ObjectId id, int amount = 1)
        {
            if (amount <= 0)
                return;
            if (_items.ContainsKey(id))
                _items[id] += amount;
            else
                _items.Add(id, amount);
        }
        public void AddItems(Dictionary<ObjectId, int> items)
        {
            foreach (var (id, amount) in items)
                AddItem(id, amount);
        }
        public void RemoveItem(ObjectId id, int amount = 1)
        {
            if (amount <= 0)
                return;
            if (_items.ContainsKey(id))
                _items[id] -= amount;
            if (_items[id] <= 0)
                _items.Remove(id);
        }
    }
}
