using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Classes.Data.Actions;
using Shuvi.Classes.Data.Statistics;
using Shuvi.Classes.Settings;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Magic;
using Shuvi.Enums.Premium;
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
        public ObjectId? Avatar { get; set; } = null;
        public ObjectId? Banner { get; set; } = null;
        public UserBadges Bages { get; set; } = UserBadges.None;

        public PremiumAbilities PremiumAbilities { get; set; } = PremiumAbilities.None;
        public long PremiumExpires { get; set; } = 0;
        public int MoneyDonated { get; set; } = 0;

        public List<ObjectId> Images { get; set; } = new();

        public int Gold { get; set; } = 0;
        public int Dispoints { get; set; } = 0;
        public Dictionary<ObjectId, int> Inventory { get; set; } = new();

        public ObjectId? Pet { get; set; } = null;
        public ulong? MasterId { get; set; } = null;

        public UserRace Race { get; set; } = UserRace.ExMachina;
        public UserSubrace Subrace { get; set; } = UserSubrace.NoSubrace;
        public UserProfession Profession { get; set; } = UserProfession.NoProfession;

        public MagicType MagicType { get; set; } = MagicType.None;
        public string? Spell { get; set; } = null;
        public string? Skill { get; set; } = null;
        public UserFightActionsData ActionChances { get; set; } = new();

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

        public UserStatisticsData Statistics { get; set; } = new();

        public int LocationId { get; set; } = 0;
        public int RegionId { get; set; } = 0;
    }
}

