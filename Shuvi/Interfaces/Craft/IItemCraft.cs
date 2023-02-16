using MongoDB.Bson;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Requirements;

namespace Shuvi.Interfaces.Craft
{
    public interface IItemCraft
    {
        public ObjectId CraftedItemId { get; }
        public IBaseRequirements Requirements { get; }
        public IReadOnlyInventory Items { get; }

        public IItem GetCraftedItem();
    }
}
