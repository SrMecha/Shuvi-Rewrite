using MongoDB.Bson;
using Shuvi.Interfaces.Inventory;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Inventory
{
    public class Inventory : ReadOnlyInventory, IInventory
    {
        public Inventory(Dictionary<ObjectId, int> items) : base(items)
        {

        }
        public void AddItem(ObjectId id, int amount = 1)
        {
            if (amount == 0)
                return;
            if (amount < 0)
            {
                RemoveItem(id, amount * -1);
                return;
            }
            var max = ItemDatabase.GetItem(id).Max;
            if (_items.GetValueOrDefault(id, 0) + amount > max && max != -1)
                amount = max - _items.GetValueOrDefault(id, 0);
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
        public void AddItems(IReadOnlyInventory inventory)
        {
            foreach (var (id, amount) in inventory.GetItemsDictionary())
                AddItem(id, amount);
        }
        public void RemoveItem(ObjectId id, int amount = 1)
        {
            if (amount == 0)
                return;
            if (amount < 0)
            {
                AddItem(id, amount * -1);
                return;
            }
            if (_items.ContainsKey(id))
                _items[id] -= amount;
            if (_items[id] <= 0)
                _items.Remove(id);
        }
    }
}
