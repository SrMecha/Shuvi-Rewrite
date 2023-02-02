using MongoDB.Bson.Serialization.Attributes;

namespace Shuvi.Classes.Data.Map
{
    public sealed class WorldMapData
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        public MapSettingsData Settings { get; set; } = new();
        public List<MapRegionData> Regions { get; set; } = new();
    }
}
