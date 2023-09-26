using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Bonuses;
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
        public override string SkillName { get; } = "CombatMode";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("skills", "CombatMode");
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override UserProfession Profession { get; } = UserProfession.Kampfer;
        public override int UsesLeft { get; protected set; } = 1;

        protected override IActionResult OnSkillUse(ICombatEntity owner, ICombatEntity target, Language lang)
        {
            owner.Effects.Add(new EffectBase(LocalizationService.Get("effects").Get(lang).Get("СombatMode/Name"),
                5, new AllBonuses()
                {
                    Strength = (int)(owner.AllCharacteristics.GetFullAbilityPower() * 0.2f),
                    Endurance = (int)(owner.AllCharacteristics.GetFullAbilityPower() * 0.2f),
                    Agility = (int)(owner.AllCharacteristics.GetFullAbilityPower() * 0.2f)
                }));
            return new ActionResult(string.Format(LocalizationService.Get("skills").Get(lang).Get("CombatMode/Use/Success"), owner.Name));
        }
    }
}
