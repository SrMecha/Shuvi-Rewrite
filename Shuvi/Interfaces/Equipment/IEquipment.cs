using MongoDB.Bson;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Equipment
{
    public interface IEquipment
    {
        public IStaticCharacteristics GetBonuses();
        public IEnumerable<ObjectId?> GetIds();
        public IEnumerable<IItem?> GetItems();
    }
}
