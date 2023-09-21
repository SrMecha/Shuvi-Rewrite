using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Shuvi.Classes.Data.Bonuses
{
    public class FightBonusesData : StaticBonusesData
    {
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AttackDamage { get; set; } = 0;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AbilityPower { get; set; } = 0;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float Armor { get; set; } = 0;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float MagicResistance { get; set; } = 0;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float CriticalStrikeChance { get; set; } = 0;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float DodgeChance { get; set; } = 0;

        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AttackDamageMultiplier { get; set; } = 0f;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float AbilityPowerMultiplier { get; set; } = 0f;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float ArmorMultiplier { get; set; } = 0f;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float MagicResistanceMultiplier { get; set; } = 0f;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float CriticalStrikeChanceMultiplier { get; set; } = 0f;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float CriticalStrikeDamageMultiplier { get; set; } = 0f;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float DodgeChanceMultiplier { get; set; } = 0f;

    }
}
