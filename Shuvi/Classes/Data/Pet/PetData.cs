using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Classes.Settings;

namespace Shuvi.Classes.Data.Pet
{
    public sealed class PetData
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public ObjectId MasterId { get; set; } = ObjectId.Empty;
        public string Name { get; set; } = string.Empty;
        public ObjectId? ParentId { get; set; } = ObjectId.Empty;

        public int Strength { get; set; } = 1;
        public int Agility { get; set; } = 1;
        public int Luck { get; set; } = 1;
        public int Intellect { get; set; } = 1;
        public int Endurance { get; set; } = 1;

        public ObjectId? Amulet { get; set; } = null;

        public int MaxMana { get; set; } = UserSettings.StandartMana;
        public int MaxHealth { get; set; } = UserSettings.StandartHealth;

        public long ManaRegenTime { get; set; } = 1;
        public long HealthRegenTime { get; set; } = 1;

        public long TamedAt { get; set; } = 1;
    }
}
