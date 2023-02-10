using Shuvi.Interfaces.Craft;

namespace Shuvi.Interfaces.Items
{
    public interface IRecipeItem : IItem
    {
        public IItemCraft Craft { get; init; }
    }
}
