using Shuvi.Classes.Data.Drop;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Items;
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
                    if (item.Chance + (luck / 0.1f) < +random.Next(0, 1001) * 0.1f)
                        result.AddItem(item.Id);
            }
            return result;
        }
    }
}
