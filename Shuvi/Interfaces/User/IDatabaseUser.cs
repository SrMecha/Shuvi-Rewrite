using Shuvi.Enums.User;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Map;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Interfaces.User
{
    public interface IDatabaseUser
    {
        public ulong Id { get; init; }
        public IUserRating Rating { get; }
        public IUserCustomization Customization { get; }
        public IUserUpgradePoints UpgradePoints { get; }
        public IUserWallet Wallet { get; }
        public ISpellInfo Spell { get; }
        public UserRace Race { get; }
        public UserProfession Profession { get; }
        public UserBreed Breed { get; }
        public IUserPetInfo Pet { get; }
        public IUserInventory Inventory { get; }
        public IUserFightActions ActionChances { get; }
        public IUserEquipment Equipment { get; }
        public IUserCharacteristics Characteristic { get; }
        public IUserStatistics Statistics { get; }
        public IUserLocation Location { get; }
    }
}
