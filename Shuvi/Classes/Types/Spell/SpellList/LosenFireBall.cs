using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Classes.Types.Effect.EffectList;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Requirements;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Damage;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Magic;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Status;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Spell.SpellList
{
    public class LosenFireBall : SpellBase
    {
        public override string SpellName { get; } = "LosenFireBall";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("spells", "LosenFireBall");
        public override MagicType MagicType { get; } = MagicType.Losen;
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override int Cost { get; } = 3;

        public override IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            if (target.IsDodged(player, 0))
                return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("LosenFireBall/Cast/Dodge").Format(player.Name));
            var damage = (int)target.BlockDamage(player.CalculateMagicDamage() * 0.8f, DamageType.Magic);
            target.ReduceHealth(damage);
            target.Effects.Add(new EffectBase(LocalizationService.Get("effects").Get(lang).Get("Burn/Name"), 2,
                new AllBonuses()
                {
                    Health = -(int)(player.AllCharacteristics.GetFullAbilityPower() * 0.2f)
                }));
            return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("LosenFireBall/Cast/Success").Format(player.Name, damage));
        }
    }
}
