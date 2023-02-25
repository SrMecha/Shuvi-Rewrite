using MongoDB.Bson.Serialization.Attributes;

namespace Shuvi.Classes.Data.Settings
{
    public sealed class AdminsData
    {
        [BsonId]
        public string Id { get; set; } = "Admins";
        public List<ulong> AdminIds { get; set; } = new();
    }
}
