using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Classes.Types.Effect.EffectList;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Requirements;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Magic;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Status;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Spell.SpellList
{
    public class StonePlatesSpell : SpellBase
    {
        public override string SpellName { get; } = "StonePlates";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("spells", "StonePlates");
        public override MagicType MagicType { get; } = MagicType.Earth;
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override int Cost { get; } = 5;

        public override IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            player.Effects.Add(new EffectBase(LocalizationService.Get("effects").Get(lang).Get("StonePlates/Name"), 5,
                new AllBonuses()
                {
                    Endurance = (int)(player.AllCharacteristics.GetFullAbilityPower() * 0.33f)
                }));
            return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("StonePlates/Cast").Format(player.Name));
        }
    }
}
