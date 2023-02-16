using Shuvi.Classes.Data.Drop;
using Shuvi.Interfaces.Drop;

namespace Shuvi.Classes.Types.Drop
{
    public sealed class ChestDrop : IChestDrop
    {
        public IDropMoney Money { get; private set; }
        public List<IDropItem> Items { get; private set; }

        public ChestDrop(IDropMoney moneyDrop, List<IDropItem> items)
        {
            Money = moneyDrop;
            Items = items;
        }
        public ChestDrop(ChestDropData data)
        {
            Money = new DropMoney(data.Money);
            Items = new();
            foreach (var itemData in data.Items)
                Items.Add(new DropItem(itemData));
        }
    }
}
