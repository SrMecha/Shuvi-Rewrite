using Shuvi.Interfaces.Rate;

namespace Shuvi.Classes.Types.Rate
{
    public class RandomWithChance<T> : IRandomWithChance<T>
        where T : notnull
    {
        protected readonly Dictionary<T, int> _items = new();

        public RandomWithChance(Dictionary<T, int> items)
        {
            _items = items;
        }
        public T GetRandom()
        {
            var now = 0;
            var need = new Random().Next(0, _items.Values.Sum() + 1);
            foreach (var (item, chance) in _items)
            {
                now += chance;
                if (now <= need)
                    return item;
            }
            return _items.Keys.First();
        }
    }
}
