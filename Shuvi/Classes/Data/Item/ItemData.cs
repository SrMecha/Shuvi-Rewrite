﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shuvi.Enums.Rating;
using Shuvi.Enums.Item;
using Shuvi.Enums.Characteristic;
using Shuvi.Enums.Requirements;
using Shuvi.Classes.Data.Drop;
using Shuvi.Enums.Localization;

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

        public Dictionary<StaticCharacteristic, int>? Bonuses { get; set; } = null;
        public Dictionary<BaseRequirement, int>? Needs { get; set; } = null;
        public Dictionary<DynamicCharacteristic, int>? PotionRecover { get; set; } = null;
        public ChestDropData? ChestDrop { get; set; } = null;
    }
}
