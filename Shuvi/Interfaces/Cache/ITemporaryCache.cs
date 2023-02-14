namespace Shuvi.Interfaces.Cache
{
    public interface ITemporaryCache<TId, TItem>
    {
        public bool TryAdd(TId id, TItem item);
        public bool TryRemove(TId id);
        public bool TryGet(TId id, out TItem? item);
        public TItem Get(TId id);
        public void DeleteNotUsedCache();
    }
}
