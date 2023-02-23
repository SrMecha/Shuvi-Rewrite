using Shuvi.Classes.Data.User;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Classes.Types.Customization;
using Shuvi.Classes.Types.Equipment;
using Shuvi.Classes.Types.Inventory;
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
using Shuvi.Interfaces.Map;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.Premium;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Statistics;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.User
{
    public class DatabaseUser : IDatabaseUser
    {
        public ulong Id { get; init; }
        public IUserRating Rating { get; private set; }
        public IUserCustomization Customization { get; private set; }
        public IUserPremium Premium { get; private set; }
        public IUserUpgradePoints UpgradePoints { get; private set; }
        public IUserWallet Wallet { get; private set; }
        public IChangableSpellInfo Spell { get; private set; }
        public IChangableSkillInfo Skill { get; private set; }
        public UserRace Race { get; private set; }
        public UserProfession Profession { get; private set; }
        public UserBreed Breed { get; private set; }
        public IUserPetInfo Pet { get; private set; }
        public IUserInventory Inventory { get; private set; }
        public IUserFightActions ActionChances { get; private set; }
        public IUserEquipment Equipment { get; private set; }
        public IUserCharacteristics Characteristics { get; private set; }
        public IUserStatistics Statistics { get; private set; }
        public IUserLocation Location { get; private set; }
    
        public DatabaseUser(UserData data, Language lang = Language.Eng)
        {
            Id = data.Id;
            Rating = new UserRating(data.Rating, lang);
            Customization = new UserCustomization(data.Color, data.Avatar, data.Banner, data.Images, data.Bages);
            Premium = new UserPremium(data.PremiumAbilities, data.PremiumExpires, data.MoneyDonated);
            Characteristics = new UserCharacteristics(
                new StaticCharacteristics(data.Strength, data.Agility, data.Luck, data.Intellect, data.Endurance),
                new RestorableCharacteristic(data.MaxHealth, data.HealthRegenTime, UserSettings.HealthPointRegenTime),
                new RestorableCharacteristic(data.MaxMana, data.ManaRegenTime, UserSettings.ManaPointRegenTime),
                data.EnergyRegenTime);
            UpgradePoints = new UserUpgradePoints(Rating, Characteristics);
            Wallet = new UserWallet(data.Gold, data.Dispoints);
            Spell = new ChangableSpellInfo(data.Spell);
            Skill = new ChangableSkillInfo(data.Skill);
            Race = data.Race;
            Profession = data.Profession;
            Breed = data.Breed;
            Pet = new UserPetInfo(data.Pet);
            Inventory = new UserInventory(data.Inventory);
            ActionChances = new UserFightActions(data.ActionChances);
            Equipment = new UserEquipment(data.Weapon, data.Helmet, data.Armor, data.Leggings, data.Boots);
            Characteristics = new UserCharacteristics(
                new StaticCharacteristics(data.Strength, data.Agility, data.Luck, data.Intellect, data.Endurance),
                new RestorableCharacteristic(data.MaxHealth, data.HealthRegenTime, UserSettings.HealthPointRegenTime),
                new RestorableCharacteristic(data.MaxMana, data.ManaRegenTime, UserSettings.ManaPointRegenTime),
                data.EnergyRegenTime);
            Statistics = new UserStatistics(data.Statistics);
            Location = new UserLocation(data.RegionId, data.LocationId);
        }
    }
}
