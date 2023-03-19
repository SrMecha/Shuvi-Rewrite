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

namespace Shuvi.Interfaces.User
{
    public interface IDatabaseUser
    {
        public ulong Id { get; init; }
        public IUserRating Rating { get; }
        public IUserCustomization Customization { get; }
        public IUserPremium Premium { get; }
        public IUserUpgradePoints UpgradePoints { get; }
        public IUserWallet Wallet { get; }
        public IMagicInfo MagicInfo { get; }
        public IChangableSpellInfo Spell { get; }
        public IChangableSkillInfo Skill { get; }
        public UserRace Race { get; }
        public UserProfession Profession { get; }
        public UserSubrace Subrace { get; }
        public IUserPetInfo Pet { get; }
        public IUserInventory Inventory { get; }
        public IUserFightActions ActionChances { get; }
        public IUserEquipment Equipment { get; }
        public IUserCharacteristics Characteristics { get; }
        public IUserStatistics Statistics { get; }
        public IUserLocation Location { get; }
    }
}
