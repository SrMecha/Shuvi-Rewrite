using Shuvi.Enums.Localization;

namespace Shuvi.Interfaces.Inventory
{
    public interface IDropInventory : IInventory
    {
        public string GetDropInfo(Language lang);
    }
}
