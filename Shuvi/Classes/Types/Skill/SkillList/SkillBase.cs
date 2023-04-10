using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Requirements;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Status;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Skill.SkillList
{
    public class SkillBase : ISkill
    {
        public ILocalizedInfo Info { get; } = new CachedLocalizedInfo("skills", "none");
        public IBaseRequirements Requirements { get; } = new BaseRequirements();
        public int UsesLeft { get; protected set; } = 0;

        public virtual bool CanUse(ICombatEntity owner)
        {
            return UsesLeft > 0;
        }
        protected virtual IActionResult OnSkillUse(ICombatEntity owner, ICombatEntity target, Language lang)
        {
            return new ActionResult(string.Format(LocalizationService.Get("skills").Get(lang).Get("none/use"), owner.Name));
        }
        public IActionResult UseSkill(ICombatEntity owner, ICombatEntity target, Language lang)
        {
            UsesLeft--;
            return OnSkillUse(owner, target, lang);
        }
        public ISkill CreateCopy()
        {
            return (ISkill)MemberwiseClone();
        }
        public virtual IActionResult? Update(ICombatEntity owner, Language lang)
        {
            return null;
        }
        public virtual bool HaveSkill()
        {
            return true;
        }
    }
}
