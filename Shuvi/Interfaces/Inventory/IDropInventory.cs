using Shuvi.Enums.Localization;
using Shuvi.Enums.Money;

namespace Shuvi.Interfaces.Inventory
{
    public interface IDropInventory : IInventory
    {
        public void AddMoney(MoneyType type, int amount);
        public string GetDropInfo(Language lang);
        public IEnumerable<KeyValuePair<MoneyType, int>> GetMoney();
    }
}
