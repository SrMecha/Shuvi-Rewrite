using Shuvi.Interfaces.Characteristics.Static;
using Shuvi.Interfaces.Combat;

namespace Shuvi.Interfaces.Effect
{
    public interface IEffect
    {
        public string Name { get; }
        public int TimeLeft { get; }
        public IStaticCharacteristics Bonuses { get; }
        protected void OnUpdate(ICombatEntity target);
        public void Update(ICombatEntity target);
    }
}
