using MongoDB.Bson;
using Shuvi.Classes.Data.Drop;
using Shuvi.Enums.Rating;

namespace Shuvi.Classes.Data.Map
{
    public sealed class MapLocationData
    {
        public string Name { get; set; } = "NoNameProvided";
        public string Description { get; set; } = "NoDescriptionProvided";
        public Rank RecomendedRank { get; set; } = Rank.E;
        public Dictionary<ObjectId, int> Enemies { get; set; } = new();
        public List<EveryDropData> MineDrop { get; set; } = new();
        public List<ObjectId> Shops { get; set; } = new();
        public List<ObjectId> Dungeons { get; set; } = new();
        public string PictureURL { get; set; } = "https://i.imgur.com/otCYNya.jpg";
    }
}
