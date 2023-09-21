using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Classes.Types.Characteristics
{
    public class EntityCharacteristics<TDynamic> : StaticCharacteristics, IEntityCharacteristics<TDynamic>
        where TDynamic : IDynamicCharacteristic
    {
        public TDynamic Health { get; set; } = default!;
        public TDynamic Mana { get; set; } = default!;

        public void Add(ICharacteristicBonuses characteristics)
        {
            Strength += characteristics.Strength;
            Agility += characteristics.Agility;
            Luck += characteristics.Luck;
            Intellect += characteristics.Intellect;
            Endurance += characteristics.Endurance;
            Health.SetMax(Health.Max + characteristics.Health);
            Mana.SetMax(Mana.Max + characteristics.Mana);
        }
    }
}
