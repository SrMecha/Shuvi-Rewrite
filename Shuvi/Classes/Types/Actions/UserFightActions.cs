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
        public override IEnumerable<(FightAction, int)> GetChances()
        {
            yield return (FightAction.LightAttack, LightAttack);
            yield return (FightAction.HeavyAttack, HeavyAttack);
            yield return (FightAction.Dodge, Dodge);
            yield return (FightAction.Defense, Defense);
            yield return (FightAction.Spell, Spell);
            yield return (FightAction.Skill, Skill);
        }
    }
}
