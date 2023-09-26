using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Interfaces.Characteristics.Bonuses;

namespace Shuvi.Classes.Types.Characteristics.Bonuses
{
    public class CharacteristicBonuses : StaticCharacteristics, ICharacteristicBonuses
    {
        public int Health { get; set; } = 0;
        public int Mana { get; set; } = 0;
        public int Energy { get; set; } = 0;

        public void Add(ICharacteristicBonuses bonuses)
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
