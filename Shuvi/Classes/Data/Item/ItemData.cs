using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shuvi.Enums.Rating;
using Shuvi.Enums.Item;
using Shuvi.Enums.Characteristic;
using Shuvi.Enums.Requirements;
using Shuvi.Classes.Data.Drop;

namespace Shuvi.Classes.Data.Item
{
    public sealed class ItemData
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public string Name { get; set; } = "NoNameProvided";
        public string Description { get; set; } = "NoDescriptionProvided";
        public ItemType Type { get; set; } = ItemType.Simple;
        public Rank Rank { get; set; } = Rank.E;
        public bool CanTrade { get; set; } = false;
        public bool CanLoose { get; set; } = true;
        public int Max { get; set; } = -1;

        public Dictionary<StaticCharacteristic, int>? Bonuses { get; set; } = null;
        public Dictionary<BaseRequirements, int>? Needs { get; set; } = null;
        public Dictionary<DynamicCharacteristic, int>? PotionRecover { get; set; } = null;
        public ChestDropData? ChestDrop { get; set; } = null;
    }
}
