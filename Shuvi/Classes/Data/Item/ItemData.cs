using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Classes.Data.Bonuses;
using Shuvi.Classes.Data.Drop;
using Shuvi.Classes.Data.Requirements;
using Shuvi.Enums.Item;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;

namespace Shuvi.Classes.Data.Item
{
    public sealed class ItemData
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public Dictionary<Language, string> Name { get; set; } = new();
        public Dictionary<Language, string> Description { get; set; } = new();
        public ItemType Type { get; set; } = ItemType.Simple;
        public Rank Rank { get; set; } = Rank.E;
        public bool CanTrade { get; set; } = false;
        public bool CanLoose { get; set; } = true;
        public int Max { get; set; } = -1;

        public AllBonusesData? Bonuses { get; set; } = null;
        public RequirementsData? Needs { get; set; } = null;
        public DynamicBonusesData? PotionRecover { get; set; } = null;
        public CraftData? Craft { get; set; } = null;
        public List<DropItemData>? ChestDrop { get; set; } = null;
    }
}
