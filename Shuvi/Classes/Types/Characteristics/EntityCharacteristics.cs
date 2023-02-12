using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Classes.Types.Characteristics
{
    public class EntityCharacteristics<TDynamic> : StaticCharacteristics, IEntityCharacteristics<TDynamic>
        where TDynamic : IDynamicCharacteristic
    {
        public TDynamic Health { get; private set; }
        public TDynamic Mana { get; private set; }

        public EntityCharacteristics(IStaticCharacteristics characteristics, TDynamic health, TDynamic mana)
            : base(characteristics)
        {
            Health = health;
            Mana = mana;
        }
        public EntityCharacteristics(int strength, int agility, int luck, int intellect, int endurance, TDynamic health, TDynamic mana)
            : base(strength, agility, luck, intellect, endurance)
        {
            Health = health;
            Mana = mana;
        }
    }
}
