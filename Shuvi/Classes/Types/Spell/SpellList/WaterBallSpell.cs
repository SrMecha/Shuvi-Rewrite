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
    public class WaterBallSpell: SpellBase
    {
        public override string SpellName { get; } = "waterBall";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("spells", "waterBall");
        public override MagicType MagicType { get; } = MagicType.Water;
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override int Cost { get; } = 3;

        public override IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            if (target.IsDodged(player, 0))
                return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("waterBall/cast/dodge").Format(player.Name));
            var damage = target.BlockDamage(player.CalculateMagicDamage());
            target.ReduceHealth(damage);
            return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("waterBall/cast/success").Format(player.Name, damage));
        }
    }
}
