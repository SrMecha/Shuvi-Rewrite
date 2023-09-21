namespace Shuvi.Interfaces.Characteristics.Bonuses
{
    public interface IDynamicBonuses
    {
        public int Health { get; }
        public int Mana { get; }
        public int Energy { get; }

        public void Add(IDynamicBonuses bonuses);
        public IEnumerable<(string, int)> GetDynamicBonuses();
    }
}
