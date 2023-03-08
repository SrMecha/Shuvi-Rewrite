using MongoDB.Bson;
using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Enums.Item;
using Shuvi.Interfaces.Characteristics.Static;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Items;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Equipment
{
    public abstract class Equipment : IEquipment
    {
        public virtual IStaticCharacteristics GetBonuses()
        {
            var result = new StaticCharacteristics(0, 0, 0, 0, 0);
            foreach (var item in GetEquipments())
                if (item is not null)
                    result.Add(item.Bonuses);
            return result;
        }
        public virtual IEnumerable<ObjectId?> GetIds()
        {
            return Enumerable.Empty<ObjectId?>();
        }
        public virtual void SetEquipment(ItemType type, ObjectId? id)
        {

        }
        public virtual ObjectId? GetEquipmentId(ItemType equipment)
        {
            return null;
        }
        public virtual IEnumerable<IEquipmentItem?> GetEquipments()
        {
            foreach (var id in GetIds())
                yield return id is null ? null : ItemDatabase.GetItem<IEquipmentItem>((ObjectId)id);
        }
    }
}
