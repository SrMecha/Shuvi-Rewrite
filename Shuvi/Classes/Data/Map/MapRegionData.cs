using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;

namespace Shuvi.Classes.Data.Map
{
    public sealed class MapRegionData
    {
        public Dictionary<Language, string> Name { get; set; } = new();
        public Dictionary<Language, string> Description { get; set; } = new();
        public Rank NeededRank { get; set; } = Rank.E;
        public Rank RecomendedRank { get; set; } = Rank.E;
        public List<MapLocationData> Locations { get; set; } = new();
        public string PictureURL { get; set; } = "https://i.imgur.com/otCYNya.jpg";

    }
}
