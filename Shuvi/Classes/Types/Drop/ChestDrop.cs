using Shuvi.Classes.Data.Drop;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Inventory;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Drop
{
    public sealed class ChestDrop : ItemsDrop, IChestDrop
    {
        public IDropMoney Money { get; private set; }

        public ChestDrop(IDropMoney moneyDrop, List<IDropItem> items) : base(items)
        {
            Money = moneyDrop;
        }
        public ChestDrop(ChestDropData data)
        {
            Money = new DropMoney(data.Money);
            foreach (var itemData in data.Items)
                Items.Add(new DropItem(itemData));
        }
        public override IDropInventory GetDrop(int luck)
        {
            var result = new DropInventory();
            var random = new Random();
            foreach (var (moneyType, amount) in Money)
                result.AddMoney(moneyType, random.Next(amount.Min, amount.Max + 1));
            foreach (var item in Items)
            {
                result.AddItem(item.Id, item.Min);
                for (int i = 0; i < item.Max - item.Min; i++)
                    if (item.Chance + (luck * 0.1f) >= +random.Next(0, 10001) * 0.01f)
                        result.AddItem(item.Id);
            }
            return result;
        }
        public override string GetChancesInfo(int luck, Language lang)
        {
            var result = new List<string>
            {
                Money.GetChancesInfo(lang)
            };
            foreach (var item in Items)
                result.Add($"{ItemDatabase.GetItem(item.Id).Info.GetName(lang)} | {(item.Min == item.Max ? $"x{item.Min}" : $"x{item.Min}-{item.Max}")} |" +
                    $" {item.Chance + (luck * 0.1f)}%");
            return string.Join("\n", result);
        }
    }
}
