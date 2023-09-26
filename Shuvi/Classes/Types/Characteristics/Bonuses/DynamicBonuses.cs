using Shuvi.Classes.Data.Bonuses;
using Shuvi.Interfaces.Characteristics.Bonuses;

namespace Shuvi.Classes.Types.Characteristics.Bonuses
{
    public class DynamicBonuses : IDynamicBonuses
    {
        public int Health { get; set; } = 0;
        public int Mana { get; set; } = 0;
        public int Energy { get; set; } = 0;

        public DynamicBonuses(DynamicBonusesData data)
        {
            Health += data.Health;
            Mana += data.Mana;
            Energy += data.Energy;
        }

        public void Add(IDynamicBonuses bonuses)
        {
            Health += bonuses.Health;
            Mana += bonuses.Mana;
            Energy += bonuses.Energy;
        }
        public IEnumerable<(string, int)> GetDynamicBonuses()
        {
            yield return ("Health", Health);
            yield return ("Mana", Mana);
            yield return ("Energy", Energy);
        }
    }
}
