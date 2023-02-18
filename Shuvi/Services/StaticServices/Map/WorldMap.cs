using Shuvi.Classes.Data.Map;
using Shuvi.Classes.Types.Map;
using Shuvi.Interfaces.Map;

namespace Shuvi.Services.StaticServices.Map
{
    public static class WorldMap
    {
        public static List<IMapRegion> Regions { get; private set; } = new();
        public static IMapSettings Settings { get; private set; } = new MapSettings(new());

        public static void Init(WorldMapData data)
        {
            Regions = ConfigureRegions(data.Regions);
            Settings = new MapSettings(data.Settings);
        }
        private static List<IMapRegion> ConfigureRegions(List<MapRegionData> regions)
        {
            var result = new List<IMapRegion>();
            foreach (var data in regions)
                result.Add(new MapRegion(data));
            return result;
        }
        public static IMapRegion GetRegion(int index)
        {
            if (Regions.Count >= index)
                return new MapRegion(new());
            return Regions[index];
        }
    }
}
