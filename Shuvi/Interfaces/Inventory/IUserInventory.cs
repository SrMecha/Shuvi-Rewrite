using MongoDB.Bson;

namespace Shuvi.Interfaces.Inventory
{
    public interface IUserInventory : IInventory
    {
        public void Clear();
        public Dictionary<ObjectId, int> GetItemsCache();
    }
}
