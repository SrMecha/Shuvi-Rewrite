using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shuvi.Classes.Data.Item
{
    public sealed class WeaponSettingsData
    {
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float DamageMultiplier { get; set; } = 1.0f;
    }
}
