using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Inventory;

namespace Shuvi.Interfaces.Drop
{
    public interface IItemsDrop
    {
        public List<IDropItem> Items { get; }

        public IDropInventory GetDrop(int luck);
        public string GetChancesInfo(int luck, Language lang);
    }
}
