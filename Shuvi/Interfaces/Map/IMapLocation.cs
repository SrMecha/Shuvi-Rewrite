using MongoDB.Bson;
using Shuvi.Classes.Types.Drop;
using Shuvi.Classes.Types.Rate;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Map
{
    public interface IMapLocation
    {
        public ILocalizedInfo Info { get; init; }
        public Rank RecomendedRank { get; init; }
        public EnemiesList Enemies { get; init; }
        public ItemsDrop MineDrop { get; init; }
        public List<ObjectId> Shops { get; init; }
        public List<ObjectId> Dungeons { get; init; }
        public string PictureURL { get; init; }

    }
}
