using MongoDB.Bson;
using Shuvi.Interfaces.Equipment;

namespace Shuvi.Classes.Types.Equipment
{
    public class UserEquipment : Equipment, IUserEquipment
    {
        public ObjectId? Weapon { get; set; }
        public ObjectId? Helmet { get; set; }
        public ObjectId? Armor { get; set; }
        public ObjectId? Leggings { get; set; }
        public ObjectId? Boots { get; set; }

        public UserEquipment(ObjectId? weapon, ObjectId? helmet, ObjectId? armor, ObjectId? leggings, ObjectId? boots)
        {
            Weapon = weapon;
            Helmet = helmet;
            Armor = armor;
            Leggings = leggings;
            Boots = boots;
        }
        public override IEnumerable<ObjectId?> GetIds()
        {
            yield return Weapon;
            yield return Helmet;
            yield return Armor;
            yield return Leggings;
            yield return Boots;
        }
    }
}
