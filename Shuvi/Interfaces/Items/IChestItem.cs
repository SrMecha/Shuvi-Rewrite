using Shuvi.Interfaces.Drop;

namespace Shuvi.Interfaces.Items
{
    public interface IChestItem : IItem
    {
        public IItemsDrop Drop { get; init; }
    }
}
