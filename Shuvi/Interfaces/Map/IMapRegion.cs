using Shuvi.Classes.Types.Localization;
using Shuvi.Enums.Rating;

namespace Shuvi.Interfaces.Map
{
    public interface IMapRegion
    {
        public LocalizedInfo Info { get; init; }
        public Rank NeededRank { get; init; }
        public Rank RecomendedRank { get; init; }
        public List<IMapLocation> Locations { get; init; }
        public string PictureURL { get; init; }

        public IMapLocation GetLocation(int index);
        public bool CanVisit(Rank rank);
    }
}
