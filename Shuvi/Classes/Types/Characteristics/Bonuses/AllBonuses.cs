using Shuvi.Classes.Data.Bonuses;
using Shuvi.Interfaces.Characteristics.Bonuses;

namespace Shuvi.Classes.Types.Characteristics.Bonuses
{
    public class AllBonuses : FightBonuses, IAllBonuses
    {
        public int Health { get; set; } = 0;
        public int Mana { get; set; } = 0;
        public int Energy { get; set; } = 0;

        public AllBonuses() { }

        public AllBonuses(AllBonusesData data) : base(data)
        {
            Health = data.Health;
            Mana = data.Mana;
            Energy = data.Energy;
        }

        public void Add(IAllBonuses bonuses)
        {
            base.Add(bonuses);
            Health += bonuses.Health;
            Energy += bonuses.Energy;
            Mana += bonuses.Mana;
        }

        public void Add(IDynamicBonuses bonuses)
        {
            Health += bonuses.Health;
            Energy += bonuses.Energy;
            Mana += bonuses.Mana;
        }

        public IEnumerable<(string, int)> GetDynamicBonuses()
        {
            yield return ("Health", Health);
            yield return ("Mana", Mana);
            yield return ("Energy", Energy);
        }
    }
}
