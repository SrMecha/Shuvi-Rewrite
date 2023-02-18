using Shuvi.Classes.Data.Actions;
using Shuvi.Enums.Actions;
using Shuvi.Interfaces.Actions;

namespace Shuvi.Classes.Types.Actions
{
    public class FightActions : IFightActions
    {
        protected int _all;

        public int LightAttack { get; set; }
        public int HeavyAttack { get; set; }
        public int Dodge { get; set; }
        public int Defense { get; set; }
        public int Spell { get; set; }

        public FightActions(FightActionsData data)
        {
            LightAttack = data.LightAttack;
            HeavyAttack = data.HeavyAttack;
            Dodge = data.Dodge;
            Defense = data.Defense;
            Spell = data.Spell;
            _all = LightAttack + HeavyAttack + Dodge + Defense + Spell;
        }
        public FightAction GetRandomAction()
        {
            var now = 0;
            var need = new Random().Next(0, _all + 1);
            foreach(var(action, chance) in GetChances())
            {
                now += chance;
                if (now <= need)
                    return action;
            }
            return FightAction.LightAttack;
        }
        public virtual IEnumerable<(FightAction, int)> GetChances()
        {
            yield return (FightAction.LightAttack, LightAttack);
            yield return (FightAction.HeavyAttack, HeavyAttack);
            yield return (FightAction.Dodge, Dodge);
            yield return (FightAction.Defense, Defense);
            yield return (FightAction.Spell, Spell);
        }
    }
}
