using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Classes.Data.Actions;
using Shuvi.Classes.Data.Statistics;
using Shuvi.Classes.Settings;
using Shuvi.Enums.Rating;

namespace Shuvi.Classes.Data.Pet
{
    public sealed class PetData
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public ulong MasterId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public ObjectId? ParentId { get; set; } = ObjectId.Empty;

        public Rank Rank { get; set; } = Rank.E;

        public int Strength { get; set; } = 1;
        public int Agility { get; set; } = 1;
        public int Luck { get; set; } = 1;
        public int Intellect { get; set; } = 1;
        public int Endurance { get; set; } = 1;

        public string? Spell { get; set; } = null;
        public FightActionsData ActionChances { get; set; } = new();

        public ObjectId? Amulet { get; set; } = null;

        public int MaxMana { get; set; } = UserSettings.StandartMana;
        public int MaxHealth { get; set; } = UserSettings.StandartHealth;

        public long ManaRegenTime { get; set; } = 1;
        public long HealthRegenTime { get; set; } = 1;

        public PetStatisticsData Statistics { get; set; } = new();
    }
}
