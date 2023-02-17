using MongoDB.Bson;
using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Classes.Types.Requirements;
using Shuvi.Interfaces.Craft;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Requirements;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Craft
{
    public class ItemCraft : IItemCraft
    {
        public ObjectId CraftedItemId { get; private set; }
        public IBaseRequirements Requirements { get; private set; }
        public IReadOnlyInventory Items { get; private set; }

        public ItemCraft(CraftData data)
        {
            CraftedItemId = data.CraftedItemId;
            Requirements = new BaseRequirements(data.Needs);
            Items = new ReadOnlyInventory(data.Items);
        }
        public IItem GetCraftedItem()
        {
            return ItemDatabase.GetItem(CraftedItemId);
        }
    }
}
