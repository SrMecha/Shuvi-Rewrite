using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Static;
using Shuvi.Interfaces.Combat;

namespace Shuvi.Interfaces.Effect
{
    public interface IEffect
    {
        public string Name { get; }
        public int TimeLeft { get; }
        public IBonusesCharacteristics Bonuses { get; }
        public void Update(ICombatEntity target);
    }
}
