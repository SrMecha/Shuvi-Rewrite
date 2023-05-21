using Shuvi.Enums.Localization;
using Shuvi.Enums.User;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Skill
{
    public interface ISkill
    {
        public string SkillName { get; }
        public ILocalizedInfo Info { get; }
        public IBaseRequirements Requirements { get; }
        public UserProfession Profession { get; }
        public int UsesLeft { get; }

        public bool CanUse(ICombatEntity owner);
        public IActionResult UseSkill(ICombatEntity owner, ICombatEntity target, Language lang);
        public IActionResult? Update(ICombatEntity owner, Language lang);
        public ISkill CreateCopy();
        public bool HaveSkill();
    }
}
