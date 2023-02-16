using MongoDB.Bson;

namespace Shuvi.Interfaces.Drop
{
    public interface IDropItem
    {
        public ObjectId Id { get; }
        public float Chance { get; }
        public int Max { get; }
        public int Min { get; }
    }
}
