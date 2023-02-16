using MongoDB.Bson;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Pet;
using Shuvi.Classes.Data.Pet;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Classes.Types.Pet
{
    public class DatabasePet : IDatabasePet
    {
        public ObjectId Id { get; private set; }
        public string Name { get; private set; }
        public IPetMasterInfo Master { get; private set; }
        public IPetParentInfo Parent { get; private set; }
        public IEntityCharacteristics<IRestorableCharacteristic> Characteristics { get; private set; }
        public IPetEquipment Equipment { get; private set; }
        public IPetStatistics Statistics { get; private set; }

        public DatabasePet(PetData data)
        {
            Id = data.Id;
            Name = data.Name;
            Master = new PetMasterInfo(data.MasterId);

        }
    }
}
