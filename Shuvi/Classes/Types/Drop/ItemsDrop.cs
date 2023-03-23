using Shuvi.Classes.Data.Drop;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Inventory;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Drop
{
    public class ItemsDrop : IItemsDrop
    {
        public List<IDropItem> Items { get; protected set; } = new();

        public ItemsDrop(List<IDropItem> items)
        {
            Items = items;
        }
        public ItemsDrop(List<DropItemData> data)
        {
            foreach (var dropData in data)
                Items.Add(new DropItem(dropData));
        }
        public ItemsDrop() { }
        public virtual IDropInventory GetDrop(int luck)
        {
            var result = new DropInventory();
            var random = new Random();
            foreach (var item in Items)
            {
                result.AddItem(item.Id, item.Min);
                for (int i = 0; i < item.Max - item.Min; i++)
                    if (item.Chance + (luck * 0.1f) >= +random.Next(0, 10001) * 0.01f)
                        result.AddItem(item.Id);
            }
            return result;
        }
        public virtual string GetChancesInfo(int luck, Language lang)
        {
            var result = new List<string>();
            foreach (var item in Items)
                result.Add($"{ItemDatabase.GetItem(item.Id).Info.GetName(lang)} | {(item.Min == item.Max ? $"x{item.Min}" : $"x{item.Min}-{item.Max}")} |" +
                    $" {item.Chance + (luck * 0.1f)}%");
            return string.Join("\n", result);
        }
    }
}
