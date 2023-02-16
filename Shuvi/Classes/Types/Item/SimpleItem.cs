using MongoDB.Bson;
using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Localization;
using Shuvi.Enums.Item;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Classes.Types.Item
{
    public class SimpleItem : IItem
    {
        public ObjectId Id { get; init; }
        public ILocalizedInfo Info { get; init; }
        public ItemType Type { get; init; }
        public Rank Rank { get; init; }
        public bool CanTrade { get; init; }
        public bool CanLoose { get; init; }
        public int Max { get; init; }

        public SimpleItem(ItemData data)
        {
            Id = data.Id;
            Info = new LocalizedInfo(data.Name, data.Description);
            Type = data.Type;
            Rank = data.Rank;
            CanTrade = data.CanTrade;
            CanLoose = data.CanLoose;
            Max = data.Max;
        }
    }
}
