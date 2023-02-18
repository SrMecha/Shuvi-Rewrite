using MongoDB.Bson;
using Shuvi.Interfaces.Equipment;

namespace Shuvi.Classes.Types.Equipment
{
    public class PetEquipment : Equipment, IPetEquipment
    {
        public ObjectId? Amulet { get; set; }

        public PetEquipment(ObjectId? amulet)
        {
            Amulet = amulet;
        }
        public override IEnumerable<ObjectId?> GetIds()
        {
            yield return Amulet;
        }
    }
}
