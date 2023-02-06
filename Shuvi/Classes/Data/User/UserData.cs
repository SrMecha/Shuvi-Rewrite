using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Classes.Data.Actions;
using Shuvi.Classes.Settings;
using Shuvi.Enums.User;

namespace Shuvi.Classes.Data.User
{

    [BsonIgnoreExtraElements]
    public sealed class UserData
    {
        [BsonId]
        public ulong Id { get; set; } = 0;
        public int Rating { get; set; } = 0;

        public uint Color { get; set; } = UserSettings.StandartColor;
        public UserImageData? Avatar { get; set; } = null;
        public UserImageData? Banner { get; set; } = null;
        public UserBages Bages { get; set; } = UserBages.None;

        public List<UserImageData> Avatars { get; set; } = new();
        public List<UserImageData> Banners { get; set; } = new();

        public int Gold { get; set; } = 0;
        public int Dispoints { get; set; } = 0;
        public Dictionary<ObjectId, int> Inventory { get; set; } = new();

        public ObjectId? Pet { get; set; } = null;
        public ulong? MasterId { get; set; } = null;

        public UserRace Race { get; set; } = UserRace.ExMachina;
        public UserBreed Breed { get; set; } = UserBreed.NoBreed;
        public UserProfession Profession { get; set; } = UserProfession.NoProfession;

        public string? Spell { get; set; } = null;
        public FightActionsData ActionChances { get; set; } = new();

        public ObjectId? Weapon { get; set; } = null;
        public ObjectId? Helmet { get; set; } = null;
        public ObjectId? Armor { get; set; } = null;
        public ObjectId? Leggings { get; set; } = null;
        public ObjectId? Boots { get; set; } = null;

        public int Strength { get; set; } = 1;
        public int Agility { get; set; } = 1;
        public int Luck { get; set; } = 1;
        public int Intellect { get; set; } = 1;
        public int Endurance { get; set; } = 1;

        public int MaxMana { get; set; } = UserSettings.StandartMana;
        public int MaxHealth { get; set; } = UserSettings.StandartHealth;

        public long ManaRegenTime { get; set; } = 1;
        public long HealthRegenTime { get; set; } = 1;
        public long EnergyRegenTime { get; set; } = 1;

        public long CreatedAt { get; set; } = 1;
        public long LiveTime { get; set; } = 1;
        public int DeathCount { get; set; } = 0;
        public int DungeonComplite { get; set; } = 0;
        public int EnemyKilled { get; set; } = 0;
        public int MaxRating { get; set; } = 0;

        public int LocationId { get; set; } = 0;
        public int RegionId { get; set; } = 0;
    }
}

