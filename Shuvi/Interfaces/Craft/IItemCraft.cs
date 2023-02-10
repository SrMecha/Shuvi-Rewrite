using MongoDB.Bson;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Requirements;

namespace Shuvi.Interfaces.Craft
{
    public interface IItemCraft
    {
        public ObjectId CraftedItem { get; }
        public IBaseRequirements Requirements { get; }
        public IReadOnlyInventory Items { get; }
    }
}
