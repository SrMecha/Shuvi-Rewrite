using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Drop
{
    public interface IItemsDrop
    {
        public List<IDropItem> Items { get; }

        public IDropInventory GetDrop(int luck);
    }
}
