﻿namespace Shuvi.Interfaces.Drop
{
    public interface IChestDrop : IItemsDrop
    {
        public IDropMoney Money { get; }
    }
}
