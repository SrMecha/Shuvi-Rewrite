using MongoDB.Bson;
using Shuvi.Enums.Item;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Craft;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Items
{
    public interface IItem
    {
        public ObjectId Id { get; init; }
        public ILocalizedInfo Info { get; init; }
        public ItemType Type { get; init; }
        public Rank Rank { get; init; }
        public bool CanTrade { get; init; }
        public bool CanLoose { get; init; }
        public int Max { get; init; }
        public IItemCraft? Craft { get; init; }
    }
}
