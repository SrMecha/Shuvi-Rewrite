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
        public FightActions(IFightActions actions)
        {
            LightAttack = actions.LightAttack;
            HeavyAttack = actions.HeavyAttack;
            Dodge = actions.Dodge;
            Defense = actions.Defense;
            Spell = actions.Spell;
            _all = CalculateAll();
        }
        public FightAction GetRandomAction()
        {
            var now = 0;
            var need = new Random().Next(0, _all + 1);
            foreach (var (action, chance) in GetChances())
            {
                now += chance;
                if (now <= need)
                    return action;
            }
            return FightAction.LightAttack;
        }
        protected virtual int CalculateAll()
        {
            return LightAttack + HeavyAttack + Dodge + Defense + Spell;
        }
        public virtual IEnumerable<(FightAction, int)> GetChances()
        {
            yield return (FightAction.LightAttack, LightAttack);
            yield return (FightAction.HeavyAttack, HeavyAttack);
            yield return (FightAction.Dodge, Dodge);
            yield return (FightAction.Defense, Defense);
            yield return (FightAction.Spell, Spell);
        }
        public virtual void SetAction(FightAction action, int rate)
        {
            switch (action)
            {
                case FightAction.LightAttack:
                    LightAttack = rate;
                    break;
                case FightAction.HeavyAttack:
                    HeavyAttack = rate;
                    break;
                case FightAction.Dodge:
                    Dodge = rate;
                    break;
                case FightAction.Defense:
                    Defense = rate;
                    break;
                case FightAction.Spell:
                    Spell = rate;
                    break;
            }
        }
        public virtual int GetAction(FightAction action)
        {
            return action switch
            {
                FightAction.LightAttack => LightAttack,
                FightAction.HeavyAttack => HeavyAttack,
                FightAction.Dodge => Dodge,
                FightAction.Defense => Defense,
                FightAction.Spell => Spell,
                _ => 0
            };
        }
    }
}
