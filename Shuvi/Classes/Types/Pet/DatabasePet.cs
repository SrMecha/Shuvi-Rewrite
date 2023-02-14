using MongoDB.Bson;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Pet;
using Shuvi.Classes.Data.Pet;

namespace Shuvi.Classes.Types.Pet
{
    public class DatabasePet : IDatabasePet
    {
        public ObjectId Id { get; private set; }
        public string Name { get; private set; }
        public ObjectId? ParentId { get; private set; }
        public ObjectId MasterId { get; private set; }
        public IEntityCharacteristics<IRestorableCharacteristic> Characteristics { get; private set; }
        public IPetEquipment Equipment { get; private set; }

        public DatabasePet(PetData data)
        {
            Id = data.Id;
            Name = data.Name;
            ParentId = data.ParentId;

        }
    }
}
