using Shuvi.Enums.Localization;
using Shuvi.Enums.Money;
using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Inventory
{
    public interface IDropInventory : IInventory
    {
        public string GetDropInfo(Language lang);
        public IEnumerable<IItem> GetItems();
        public IEnumerable<Dictionary<MoneyType, int>> GetMoney();
    }
}
