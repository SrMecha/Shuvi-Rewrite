using Shuvi.Classes.Data.Item;
using Shuvi.Interfaces.Craft;
using Shuvi.Interfaces.Items;

namespace Shuvi.Classes.Types.Item
{
    public class RecipeItem : SimpleItem, IRecipeItem
    {
        public IItemCraft Craft { get; init; }

        public RecipeItem(ItemData data) : base(data)
        {

        }
    }
}
