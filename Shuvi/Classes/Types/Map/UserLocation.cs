using Shuvi.Interfaces.Map;
using Shuvi.Services.StaticServices.Map;

namespace Shuvi.Classes.Types.Map
{
    public class UserLocation : IUserLocation
    {
        public int LocationId { get; private set; }
        public int RegionId { get; private set; }

        public UserLocation(int regionId, int locationId)
        {
            LocationId = locationId;
            RegionId = regionId;
        }
        public IMapLocation GetLocation()
        {
            return GetRegion().GetLocation(LocationId);
        }
        public IMapRegion GetRegion()
        {
            return WorldMap.GetRegion(RegionId);
        }
        public void SetLocation(int locationId)
        {
            LocationId = locationId;
        }
        public void SetRegion(int regionId, int locationId = 0)
        {
            RegionId = regionId;
            LocationId = locationId;
        }
    }
}
