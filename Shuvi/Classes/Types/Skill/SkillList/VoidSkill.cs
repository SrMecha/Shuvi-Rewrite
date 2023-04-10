using Shuvi.Interfaces.Combat;

namespace Shuvi.Classes.Types.Skill.SkillList
{
    public class VoidSkill : SkillBase
    {
        public override bool CanUse(ICombatEntity owner)
        {
            return false;
        }
        public override bool HaveSkill()
        {
            return false;
        }
    }
}
