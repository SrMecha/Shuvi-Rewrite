using MongoDB.Bson;

namespace Shuvi.Interfaces.Equipment
{
    public interface IUserEquipment : IEquipment
    {
        public ObjectId? Weapon { get; set; }
        public ObjectId? Helmet { get; set; }
        public ObjectId? Armor { get; set; }
        public ObjectId? Leggings { get; set; }
        public ObjectId? Boots { get; set; }
    }
}
