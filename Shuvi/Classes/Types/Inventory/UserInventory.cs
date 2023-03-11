using MongoDB.Bson;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Items;

namespace Shuvi.Classes.Types.Inventory
{
    public class UserInventory : Inventory, IUserInventory
    {
        public UserInventory(Dictionary<ObjectId, int> items) : base(items) { }
        public void Clear()
        {
            IItem item;
            for (int i = Count; i > 0; i--)
            {
                item = GetItemAt(i - 1);
                if (item.CanLoose)
                    _items.Remove(item.Id);
            }
        }
    }
}
