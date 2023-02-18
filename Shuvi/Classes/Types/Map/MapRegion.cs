using Shuvi.Classes.Data.Map;
using Shuvi.Classes.Types.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Map;

namespace Shuvi.Classes.Types.Map
{
    public class MapRegion : IMapRegion
    {
        public LocalizedInfo Info { get; init; }
        public Rank NeededRank { get; init; }
        public Rank RecomendedRank{ get; init; }
        public List<IMapLocation> Locations { get; init; }
        public string PictureURL { get; init; }

        public MapRegion(MapRegionData data)
        {
            Info = new LocalizedInfo(data.Name, data.Description);
            NeededRank = data.NeededRank;
            RecomendedRank = data.RecomendedRank;
            Locations = ConfigureLocations(data.Locations);
            PictureURL = data.PictureURL;
        }
        private static List<IMapLocation> ConfigureLocations(List<MapLocationData> locations)
        {
            var result = new List<IMapLocation>();
            foreach (var data in locations)
                result.Add(new MapLocation(data));
            return result;
        }
        public bool CanVisit(Rank rank)
        {
            return (int)NeededRank <= (int)rank;
        }
        public IMapLocation GetLocation(int index)
        {
            if (Locations.Count >= index)
                return new MapLocation(new());
            return Locations[index];
        }
    }
}
