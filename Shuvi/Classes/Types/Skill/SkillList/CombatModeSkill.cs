using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Effect.EffectList;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Requirements;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Localization;
using Shuvi.Enums.User;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Status;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Skill.SkillList
{
    public class CombatModeSkill : SkillBase
    {
        public override string SkillName { get; } = "combatMode";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("skills", "combatMode");
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override UserProfession Profession { get; } = UserProfession.Kampfer;
        public override int UsesLeft { get; protected set; } = 1;

        protected override IActionResult OnSkillUse(ICombatEntity owner, ICombatEntity target, Language lang)
        {
            owner.Effects.Add(new EffectBase(LocalizationService.Get("effects").Get(lang).Get("combatMode/name"),
                5, new BonusesCharacteristics(strength: (owner.Characteristics.Intellect + owner.EffectBonuses.Intellect) / 6 + 1,
                endurance: (owner.Characteristics.Intellect + owner.EffectBonuses.Intellect) / 6 + 1,
                agility: (owner.Characteristics.Intellect + owner.EffectBonuses.Intellect) / 6 + 1)));
            return new ActionResult(string.Format(LocalizationService.Get("skills").Get(lang).Get("combatMode/use/success"), owner.Name));
        }
    }
}
