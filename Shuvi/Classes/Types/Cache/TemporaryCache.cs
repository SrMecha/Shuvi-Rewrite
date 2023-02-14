using Discord;
using Shuvi.Interfaces.Cache;

namespace Shuvi.Classes.Types.Cache
{
    public class TemporaryCache<TId, TItem> : ITemporaryCache<TId, TItem>
        where TId: notnull
    {
        private readonly int _deleteCacheAfter = 600;
        private readonly Dictionary<TId, TItem> _itemCache = new();
        private readonly Dictionary<TId, long> _timeCache = new();

        public TemporaryCache(int deleteCacheAfter)
        {
            _deleteCacheAfter = deleteCacheAfter;
        }
        public TemporaryCache() { }
        public TItem Get(TId id)
        {
            return _itemCache.GetValueOrDefault(id)!;
        }
        public bool TryAdd(TId id, TItem item)
        {
            if (_timeCache.ContainsKey(id))
                _timeCache[id] = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            else
                _timeCache.TryAdd(id, ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
            return _itemCache.TryAdd(id, item);
        }
        public bool TryGet(TId id, out TItem? item)
        {
            return _itemCache.TryGetValue(id, out item);
        }
        public bool TryRemove(TId id)
        {
            _timeCache.Remove(id);
            return _itemCache.Remove(id);
        }
        public void DeleteNotUsedCache()
        {
            long currentTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            for (int i = _timeCache.Count; i >= 0; i--)
            {
                var element = _timeCache.ElementAt(i);
                if (element.Value + _deleteCacheAfter > currentTime)
                {
                    _timeCache.Remove(element.Key);
                    _itemCache.Remove(element.Key);
                }
            }
        }
    }
}
