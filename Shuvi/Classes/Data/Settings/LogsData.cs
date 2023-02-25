using MongoDB.Bson.Serialization.Attributes;

namespace Shuvi.Classes.Data.Settings
{
    public class LogsData
    {
        [BsonId]
        public string Id { get; set; } = "Logs";
        public ulong ServerLogChannelId { get; set; } = 0;
        public ulong UserLogChannelId { get; set; } = 0;
    }
}
