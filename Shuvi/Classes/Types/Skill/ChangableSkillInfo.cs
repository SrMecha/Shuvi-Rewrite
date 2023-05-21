using Shuvi.Classes.Types.Localization;
using Shuvi.Interfaces.Skill;

namespace Shuvi.Classes.Types.Skill
{
    public class ChangableSkillInfo : SkillInfo, IChangableSkillInfo
    {
        public ChangableSkillInfo(string? skillName) : base(skillName) { }
        public void SetSkill(string? skillName)
        {
            _skillName = skillName;
            Info = new CachedLocalizedInfo("skills", skillName);
        }
    }
}
