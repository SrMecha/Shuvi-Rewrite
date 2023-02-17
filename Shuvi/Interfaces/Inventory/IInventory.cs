using MongoDB.Bson;

namespace Shuvi.Interfaces.Inventory
{
    public interface IInventory : IReadOnlyInventory
    {
        public void AddItem(ObjectId id, int amount = 1);
        public void RemoveItem(ObjectId id, int amount = 1);
    }
}
