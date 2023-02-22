using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Spell.SpellList;
using Shuvi.Interfaces.Skill;

namespace Shuvi.Classes.Types.Skill
{
    public class UserSkillInfo : SkillInfo, IUserSkillInfo
    {
        public UserSkillInfo(string? skillName) : base(skillName) { }
        public void SetSkill(string? skillName)
        {
            _skillName = skillName;
            Info = new CachedLocalizedInfo("spells", skillName);
        }
    }
}
