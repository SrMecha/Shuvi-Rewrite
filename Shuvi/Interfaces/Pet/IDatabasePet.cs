using MongoDB.Bson;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Interfaces.Pet
{
    public interface IDatabasePet
    {
        public ObjectId Id { get; }
        public string Name { get; }
        public IPetMasterInfo Master { get; }
        public IPetParentInfo Parent { get; }
        public Rank Rank { get; }
        public IChangableSpellInfo Spell { get; }
        public IFightActions ActionChances { get; }
        public IEntityCharacteristics<IRestorableCharacteristic> Characteristics { get; }
        public IPetEquipment Equipment { get; }
        public IPetStatistics Statistics { get; }
    }
}
