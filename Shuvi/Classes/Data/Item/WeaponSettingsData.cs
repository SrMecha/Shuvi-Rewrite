using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Shuvi.Classes.Data.Item
{
    public sealed class WeaponSettingsData
    {
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float DamageMultiplier { get; set; } = 1.0f;
    }
}
