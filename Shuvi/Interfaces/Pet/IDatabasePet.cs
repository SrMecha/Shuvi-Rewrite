using MongoDB.Bson;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Interfaces.Pet
{
    public interface IDatabasePet
    {
        public ObjectId Id { get; }
        public string Name { get; }
        public ObjectId? ParentId { get; }
        public IEntityCharacteristics<IRestorableCharacteristic> Characteristics { get; }
        public IPetEquipment Equipment { get; }
        public IPetStatistics Statistics { get; }
    }
}
