using MongoDB.Bson;

namespace Shuvi.Interfaces.Equipment
{
    public interface IPetEquipment : IEquipment
    {
        public ObjectId? Amulet { get; set; }
    }
}
