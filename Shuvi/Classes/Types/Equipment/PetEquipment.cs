using MongoDB.Bson;
using Shuvi.Enums.Item;
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
        public override IEnumerable<(ItemType, ObjectId?)> GetIdsWithType()
        {
            yield return (ItemType.Amulet, Amulet);
        }
        public override void SetEquipment(ItemType type, ObjectId? id)
        {
            switch (type)
            {
                case ItemType.Amulet:
                    Amulet = id;
                    return;
            }
        }
        public override ObjectId? GetEquipmentId(ItemType equipment)
        {
            return equipment switch
            {
                ItemType.Amulet => Amulet,
                _ => null
            };
        }
        public override void RemoveAllEquipment()
        {
            Amulet = null;
        }
    }
}
