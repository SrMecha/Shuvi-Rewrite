using MongoDB.Bson;
using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Inventory
{
    public interface IInventory : IReadOnlyInventory
    {
        public void AddItem(IItem item);
        public void AddItem(ObjectId id, int amount);
        public void RemoveItem(ObjectId id);
        public void RemoveItem(ObjectId id, int amount);
    }
}
