﻿using MongoDB.Bson;
using Shuvi.Enums.Item;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Equipment
{
    public interface IEquipment
    {
        public IAllBonuses GetBonuses();
        public IEnumerable<ObjectId?> GetIds();
        public IEnumerable<(ItemType, ObjectId?)> GetIdsWithType();
        public void SetEquipment(ItemType type, ObjectId? id);
        public ObjectId? GetEquipmentId(ItemType equipment);
        public IEnumerable<IEquipmentItem?> GetEquipments();
        public IEnumerable<(ItemType, IEquipmentItem?)> GetEquipmentsWithType();
        public void RemoveAllEquipment();
    }
}
