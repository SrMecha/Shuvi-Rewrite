namespace Shuvi.Interfaces.Map
{
    public interface IUserLocation
    {
        public int LocationId { get; }
        public int RegionId { get; }
        public IMapLocation GetLocation();
        public IMapRegion GetRegion();
        public void SetLocation(int locationId);
        public void SetRegion(int regionId, int locationId = 0);
    }
}
