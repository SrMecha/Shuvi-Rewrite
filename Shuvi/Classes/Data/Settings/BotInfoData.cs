using MongoDB.Bson.Serialization.Attributes;

namespace Shuvi.Classes.Data.Settings
{
    public sealed class BotInfoData
    {
        [BsonId]
        public string Id { get; set; } = "Info";
        public string Version { get; set; } = "VersionNotConfigured";
        public string TosLink { get; set; } = string.Empty;
    }
}
