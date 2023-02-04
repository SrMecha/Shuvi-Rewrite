using Shuvi.Enums.User;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Map;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Interfaces.User
{
    public interface IDatabaseUser
    {
        public ulong Id { get; init; }
        public IUserRating Rating { get; }
        public IUserUpgradePoints UpgradePoints { get; }
        public IUserWallet Wallet { get; }
        public ISpellInfo Spell { get; }
        public UserRace Race { get; }
        public UserProfession Profession { get; }
        public UserBreed Breed { get; }
        public IUserInventory Inventory { get; }
        public IUserFightActions ActionChances { get; }
        public IUserEquipment Equipment { get; }
        public IAllCharacteristics Characteristic { get; }
        public IUserStatistics Statistics { get; }
        public IUserLocation Location { get; }
    }
}
