using MongoDB.Bson;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Equipment;

namespace Shuvi.Interfaces.Pet
{
    public interface IPet
    {
        public ObjectId Id { get; }
        public string Name { get; }
        public ObjectId? ParentId { get; }
        public IAllCharacteristics Characteristics { get; }
        public IPetEquipment Equipment { get; }
    }
}
