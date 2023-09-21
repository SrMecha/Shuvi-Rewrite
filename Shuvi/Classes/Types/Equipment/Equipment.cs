using MongoDB.Bson;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Enums.Item;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Items;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Equipment
{
    public abstract class Equipment : IEquipment
    {
        public virtual IAllBonuses GetBonuses()
        {
            var result = new AllBonuses();
            foreach (var item in GetEquipments())
                if (item is not null)
                    result.Add(item.Bonuses);
            return result;
        }
        public virtual IEnumerable<ObjectId?> GetIds()
        {
            return Enumerable.Empty<ObjectId?>();
        }
        public virtual IEnumerable<(ItemType, ObjectId?)> GetIdsWithType()
        {
            return Enumerable.Empty<(ItemType, ObjectId?)>();
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
        public virtual IEnumerable<(ItemType, IEquipmentItem?)> GetEquipmentsWithType()
        {
            foreach (var (type, id) in GetIdsWithType())
                yield return id is null ? (type, null) : (type, ItemDatabase.GetItem<IEquipmentItem>((ObjectId)id));
        }
        public virtual void RemoveAllEquipment()
        {

        }
    }
}
