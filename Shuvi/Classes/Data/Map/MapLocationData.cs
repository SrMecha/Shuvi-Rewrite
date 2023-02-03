using MongoDB.Bson;
using Shuvi.Classes.Data.Drop;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;

namespace Shuvi.Classes.Data.Map
{
    public sealed class MapLocationData
    {
        public Dictionary<Language, string> Name { get; set; } = new();
        public Dictionary<Language, string> Description { get; set; } = new();
        public Rank RecomendedRank { get; set; } = Rank.E;
        public Dictionary<ObjectId, int> Enemies { get; set; } = new();
        public List<EveryDropData> MineDrop { get; set; } = new();
        public List<ObjectId> Shops { get; set; } = new();
        public List<ObjectId> Dungeons { get; set; } = new();
        public string PictureURL { get; set; } = "https://i.imgur.com/otCYNya.jpg";
    }
}
