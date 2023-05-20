using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Characteristics;
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
    public class FireBallSpell : SpellBase
    {
        public override string SpellName { get; } = "FireBall";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("spells", "fireBall");
        public override MagicType MagicType { get; } = MagicType.Fire;
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override int Cost { get; } = 4;

        public override IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            if (target.IsDodged(player, 0))
                return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("fireBall/cast/dodge").Format(player.Name));
            var damage = target.BlockDamage(player.CalculateMagicDamage());
            target.ReduceHealth(damage);
            target.Effects.Add(new EffectBase(LocalizationService.Get("effects").Get(lang).Get("burn/name"), 2, 
                new BonusesCharacteristics(health: damage / 5)));
            return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("fireBall/cast/success").Format(player.Name, damage));
        }
    }
}
