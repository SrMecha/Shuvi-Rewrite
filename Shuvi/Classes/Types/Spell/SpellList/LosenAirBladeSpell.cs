using Shuvi.Classes.Extensions;
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
    public class LosenAirBladeSpell : SpellBase
    {
        public override string SpellName { get; } = "losenAirBlade";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("spells", "losenAirBlade");
        public override MagicType MagicType { get; } = MagicType.Losen;
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override int Cost { get; } = 2;

        public override IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            if (target.IsDodged(player, 30))
                return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("losenAirBlade/cast/dodge").Format(player.Name));
            var damage = target.BlockDamage(player.CalculateMagicDamage() * 0.8f);
            target.ReduceHealth(damage);
            return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("losenAirBlade/cast/success").Format(player.Name, damage));
        }
    }
}
