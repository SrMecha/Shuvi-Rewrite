using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Effect;

namespace Shuvi.Classes.Types.Effect.EffectList
{
    public class EffectBase : IEffect
    {
        public string Name { get; protected set; }
        public int TimeLeft { get; protected set; }
        public IAllBonuses Bonuses { get; protected set; }

        public EffectBase(string name, int timeLeft, IAllBonuses? bonuses = null)
        {
            Name = name;
            TimeLeft = timeLeft;
            Bonuses = bonuses ?? new AllBonuses();
        }
        protected virtual void OnUpdate(ICombatEntity target)
        {

        }
        public void Update(ICombatEntity target)
        {
            target.Characteristics.Health.Add(Bonuses.Health);
            target.Characteristics.Mana.Add(Bonuses.Mana);
            OnUpdate(target);
            TimeLeft--;
        }
    }
}
