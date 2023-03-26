using Shuvi.Classes.Data.Actions;
using Shuvi.Enums.Actions;
using Shuvi.Interfaces.Actions;

namespace Shuvi.Classes.Types.Actions
{
    public class UserFightActions : FightActions, IUserFightActions
    {
        public int Skill { get; set; }

        public UserFightActions(UserFightActionsData data) : base(data)
        {
            Skill = data.Skill;
            _all += Skill;
        }
        public UserFightActions(IUserFightActions actions) : base(actions)
        {
            Skill = actions.Skill;
            _all += CalculateAll();
        }
        protected override int CalculateAll()
        {
            return LightAttack + HeavyAttack + Dodge + Defense + Spell + Skill;
        }
        public override IEnumerable<(FightAction, int)> GetChances()
        {
            yield return (FightAction.LightAttack, LightAttack);
            yield return (FightAction.HeavyAttack, HeavyAttack);
            yield return (FightAction.Dodge, Dodge);
            yield return (FightAction.Defense, Defense);
            yield return (FightAction.Spell, Spell);
            yield return (FightAction.Skill, Skill);
        }
        public override void SetAction(FightAction action, int rate)
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
                case FightAction.Skill:
                    Skill = rate;
                    break;
            }
        }
        public override int GetAction(FightAction action)
        {
            return action switch
            {
                FightAction.LightAttack => LightAttack,
                FightAction.HeavyAttack => HeavyAttack,
                FightAction.Dodge => Dodge,
                FightAction.Defense => Defense,
                FightAction.Spell => Spell,
                FightAction.Skill => Skill,
                _ => 0
            };
        }
    }
}
