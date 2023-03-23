using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shuvi.Classes.Data.Drop
{
    [BsonNoId]
    public sealed class DropItemData
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float Chance { get; set; } = 0f;
        public int Max { get; set; } = 0;
        public int Min { get; set; } = 0;
    }
}
