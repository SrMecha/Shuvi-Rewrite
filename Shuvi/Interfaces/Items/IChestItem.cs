using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Items
{
    public interface IChestItem
    {
        public IDropInventory GetDrop(IDatabaseUser user);
    }
}
