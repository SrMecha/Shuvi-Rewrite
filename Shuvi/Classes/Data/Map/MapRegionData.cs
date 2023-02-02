using Shuvi.Enums.Rating;

namespace Shuvi.Classes.Data.Map
{
    public sealed class MapRegionData
    {
        public string Name { get; set; } = "NoNameProvided";
        public string Description { get; set; } = "NoDescriptionProvided";
        public Rank NeededRank { get; set; } = Rank.E;
        public Rank RecomendedRank { get; set; } = Rank.E;
        public List<MapLocationData> Locations { get; set; } = new();
        public string PictureURL { get; set; } = "https://i.imgur.com/otCYNya.jpg";

    }
}
