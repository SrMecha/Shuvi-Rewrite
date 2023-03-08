﻿using MongoDB.Bson;
using Shuvi.Enums.Item;
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
        public override void SetEquipment(ItemType type, ObjectId? id)
        {
            switch (type)
            {
                case ItemType.Amulet:
                    Weapon = id;
                    return;
                case ItemType.Helmet:
                    Helmet = id;
                    return;
                case ItemType.Armor:
                    Armor = id;
                    return;
                case ItemType.Leggings:
                    Leggings = id;
                    return;
                case ItemType.Boots:
                    Boots = id;
                    return;
            }
        }
        public override ObjectId? GetEquipmentId(ItemType equipment)
        {
            return equipment switch
            {
                ItemType.Weapon => Weapon,
                ItemType.Helmet => Helmet,
                ItemType.Armor => Armor,
                ItemType.Leggings => Leggings,
                ItemType.Boots => Boots,
                _ => null
            };
        }
    }
}
