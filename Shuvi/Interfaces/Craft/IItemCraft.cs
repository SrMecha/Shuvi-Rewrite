using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Requirements;

namespace Shuvi.Interfaces.Craft
{
    public interface IItemCraft
    {
        public bool IsHidden { get; }
        public bool IsCrafting { get; }
        public IBaseRequirements Requirements { get; }
        public IReadOnlyInventory Items { get; }
    }
}
