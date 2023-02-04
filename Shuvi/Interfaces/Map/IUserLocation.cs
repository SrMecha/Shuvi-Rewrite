namespace Shuvi.Interfaces.Map
{
    public interface IUserLocation
    {
        public int LocationId { get; }
        public int RegionId { get; }
        public IMapLocation GetLocation();
        public IMapRegion GetRegion();
        public void SetLocation(int index);
        public void SetRegion(int index);
    }
}
