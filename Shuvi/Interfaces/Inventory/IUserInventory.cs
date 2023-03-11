using MongoDB.Bson;

namespace Shuvi.Interfaces.Inventory
{
    public interface IUserInventory : IInventory
    {
        public void Clear();
    }
}
