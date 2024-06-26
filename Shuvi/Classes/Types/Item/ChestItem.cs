﻿using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Drop;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Items;

namespace Shuvi.Classes.Types.Item
{
    public class ChestItem : SimpleItem, IChestItem
    {
        public IChestDrop Drop { get; init; }

        public ChestItem(ItemData data) : base(data)
        {
            Drop = new ChestDrop(data.ChestDrop ?? new());
        }
    }
}
