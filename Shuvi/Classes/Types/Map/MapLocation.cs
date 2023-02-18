using MongoDB.Bson;
using Shuvi.Classes.Data.Map;
using Shuvi.Classes.Types.Drop;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Rate;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Map;

namespace Shuvi.Classes.Types.Map
{
    public class MapLocation : IMapLocation
    {
        public ILocalizedInfo Info { get; init; }
        public Rank RecomendedRank { get; init; }
        public EnemiesList Enemies { get; init; }
        public ItemsDrop MineDrop { get; init; }
        public List<ObjectId> Shops { get; init; }
        public List<ObjectId> Dungeons { get; init; }
        public string PictureURL { get; init; }

        public MapLocation(MapLocationData data)
        {
            Info = new LocalizedInfo(data.Name, data.Description);
            RecomendedRank = data.RecomendedRank;
            Enemies = new EnemiesList(data.Enemies);
            MineDrop = new ItemsDrop(data.MineDrop);
            Shops = data.Shops;
            Dungeons = data.Dungeons;
            PictureURL = data.PictureURL;
        }
    }
}
