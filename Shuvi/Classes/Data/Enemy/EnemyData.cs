﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shuvi.Enums.Rating;
using Shuvi.Classes.Data.ActionChances;
using Shuvi.Classes.Data.Drop;
using Shuvi.Enums.Localization;

namespace Shuvi.Classes.Data.Enemy
{
    public sealed class EnemyData
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public Dictionary<Language, string> Name { get; set; } = new();
        public Dictionary<Language, string> Description { get; set; } = new();
        public Rank Rank { get; set; } = Rank.E;
        public int RatingGet { get; set; } = 1;
        public int UpgradePoints { get; set; } = 5;
        public ActionChancesData ActionChances { get; set; } = new();
        public string? SpellName { get; set; } = null;
        public int Strength { get; set; } = 1;
        public int Agility { get; set; } = 1;
        public int Luck { get; set; } = 1;
        public int Intellect { get; set; } = 1;
        public int Endurance { get; set; } = 1;
        public int Mana { get; set; } = 10;
        public int Health { get; set; } = 100;
        public EveryDropData Drop { get; set; } = new();
    }
}
