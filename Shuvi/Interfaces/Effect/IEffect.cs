using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Combat;

namespace Shuvi.Interfaces.Effect
{
    public interface IEffect
    {
        public string Name { get; }
        public int TimeLeft { get; }
        public IAllBonuses Bonuses { get; }
        public void Update(ICombatEntity target);
    }
}
