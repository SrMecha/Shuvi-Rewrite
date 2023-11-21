using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Classes.Types.Customization;
using Shuvi.Classes.Types.Equipment;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Classes.Types.Magic;
using Shuvi.Classes.Types.Map;
using Shuvi.Classes.Types.Pet;
using Shuvi.Classes.Types.Premium;
using Shuvi.Classes.Types.Skill;
using Shuvi.Classes.Types.Spell;
using Shuvi.Classes.Types.Statistics;
using Shuvi.Enums.Localization;
using Shuvi.Enums.User;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Magic;
using Shuvi.Interfaces.Map;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.Premium;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Statistics;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.User
{
    public class DatabaseUser : IDatabaseUser
    {
        public ulong Id { get; init; }
        public IUserRating Rating { get; private set; }
        public Language Language { get; private set; }
        public IUserCustomization Customization { get; private set; }
        public IUserPremium Premium { get; private set; }
        public IUserUpgradePoints UpgradePoints { get; private set; }
        public IUserWallet Wallet { get; private set; }
        public IMagicInfo MagicInfo { get; private set; }
        public IChangableSpellInfo Spell { get; private set; }
        public IChangableSkillInfo Skill { get; private set; }
        public UserRace Race { get; private set; }
        public UserProfession Profession { get; private set; }
        public UserSubrace Subrace { get; private set; }
        public IUserPetInfo Pet { get; private set; }
        public IUserInventory Inventory { get; private set; }
        public IUserFightActions ActionChances { get; private set; }
        public IUserEquipment Equipment { get; private set; }
        public IUserCharacteristics Characteristics { get; private set; }
        public IUserStatistics Statistics { get; private set; }
        public IUserLocation Location { get; private set; }

        public DatabaseUser(UserData data)
        {
            Id = data.Id;
            Rating = new UserRating(this, data.Rating);
            Language = data.Language;
            Customization = new UserCustomization(data.Color, data.Avatar, data.Banner, data.Images, data.Badges);
            Premium = new UserPremium(data.PremiumAbilities, data.PremiumExpires, data.MoneyDonated);
            Characteristics = new UserCharacteristics()
            {
                Strength = data.Strength,
                Agility = data.Agility,
                Luck = data.Luck,
                Intellect = data.Intellect,
                Endurance = data.Endurance,
                Health = new RestorableCharacteristic(data.MaxHealth, data.HealthRegenTime, UserSettings.HealthPointRegenTime),
                Mana = new RestorableCharacteristic(data.MaxMana, data.ManaRegenTime, UserSettings.ManaPointRegenTime)
            };
            ((UserCharacteristics)Characteristics).Energy = new Energy(Characteristics, data.EnergyRegenTime);
            UpgradePoints = new UserUpgradePoints(Rating, Characteristics);
            Wallet = new UserWallet(data.Gold, data.Dispoints);
            MagicInfo = new MagicInfo(data.MagicType);
            Spell = new ChangableSpellInfo(data.Spell);
            Skill = new ChangableSkillInfo(data.Skill);
            Race = data.Race;
            Profession = data.Profession;
            Subrace = data.Subrace;
            Pet = new UserPetInfo(data.Pet);
            Inventory = new UserInventory(data.Inventory);
            ActionChances = new UserFightActions(data.ActionChances);
            Equipment = new UserEquipment(data.Weapon, data.Helmet, data.Armor, data.Leggings, data.Boots);
            Statistics = new UserStatistics(data.Statistics);
            Location = new UserLocation(data.RegionId, data.LocationId);
        }
        public void SetRace(UserRace race)
        {
            Race = race;
        }
        public void SetProfession(UserProfession profession)
        {
            Profession = profession;
        }
        public void SetSubrace(UserSubrace subrace)
        {
            Subrace = subrace;
        }

        public async Task SetAndSaveLanguage(Language lang)
        {
            Language = lang;
            await UserDatabase.UpdateUser(Id, new UpdateDefinitionBuilder<UserData>().Set(x => x.Language, Language));
        }
    }
}
