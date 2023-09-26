using MongoDB.Bson;
using Shuvi.Classes.Data.Pet;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Equipment;
using Shuvi.Classes.Types.Spell;
using Shuvi.Classes.Types.Statistics;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Classes.Types.Pet
{
    public class DatabasePet : IDatabasePet
    {
        public ObjectId Id { get; private set; }
        public string Name { get; private set; }
        public IPetMasterInfo Master { get; private set; }
        public IPetParentInfo Parent { get; private set; }
        public Rank Rank { get; private set; }
        public IEntityCharacteristics<IRestorableCharacteristic> Characteristics { get; private set; }
        public IPetEquipment Equipment { get; private set; }
        public IPetStatistics Statistics { get; private set; }
        public IChangableSpellInfo Spell { get; private set; }
        public IFightActions ActionChances { get; private set; }

        public DatabasePet(PetData data)
        {
            Id = data.Id;
            Name = data.Name;
            Rank = data.Rank;
            Spell = new ChangableSpellInfo(data.Spell);
            ActionChances = new FightActions(data.ActionChances);
            Master = new PetMasterInfo(data.MasterId);
            Parent = new PetParentInfo(data.ParentId);
            Characteristics = new EntityCharacteristics<IRestorableCharacteristic>()
            {
                Strength = data.Strength,
                Agility = data.Agility,
                Luck = data.Luck,
                Intellect = data.Intellect,
                Endurance = data.Endurance,
                Health = new RestorableCharacteristic(data.MaxHealth, data.HealthRegenTime, UserSettings.HealthPointRegenTime),
                Mana = new RestorableCharacteristic(data.MaxMana, data.ManaRegenTime, UserSettings.ManaPointRegenTime)
            };
            Equipment = new PetEquipment(data.Amulet);
            Statistics = new PetStatistics(data.Statistics);
        }
    }
}
