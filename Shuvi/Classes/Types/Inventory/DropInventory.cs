using Discord;
using MongoDB.Bson;
using Shuvi.Classes.Extensions;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Money;
using Shuvi.Interfaces.Inventory;
using Shuvi.Services.StaticServices.Emoji;

namespace Shuvi.Classes.Types.Inventory
{
    public class DropInventory : Inventory, IDropInventory
    {
        private Dictionary<MoneyType, int> _money;

        public DropInventory() : base(new())
        {
            _money = new();
        }
        public DropInventory(Dictionary<ObjectId, int> items, Dictionary<MoneyType, int> money) : base(items)
        {
            _money = money;
        }
        public void AddMoney(MoneyType type, int amount)
        {
            if (_money.ContainsKey(type))
                _money[type] += amount;
            else
                _money.Add(type, amount);
        }
        public string GetDropInfo(Language lang)
        {
            var result = new List<string>();
            foreach(var (moneyType, amount) in _money)
                result.Add($"+ {amount} {EmojiService.Get(moneyType.GetLowerName())}");
            foreach (var (item, amount) in GetItems())
                result.Add($"+{amount} {item.Info.GetName(lang)}");
            return string.Join("\n", result);
        }
        public IEnumerable<KeyValuePair<MoneyType, int>> GetMoney()
        {
            return _money.AsEnumerable();
        }
    }
}
