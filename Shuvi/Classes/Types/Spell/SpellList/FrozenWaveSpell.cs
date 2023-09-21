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
    public class FrozenWaveSpell : SpellBase
    {
        public override string SpellName { get; } = "FrozenWave";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("spells", "FrozenWave");
        public override MagicType MagicType { get; } = MagicType.Ice;
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override int Cost { get; } = 4;

        public override IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            if (target.IsDodged(player, 0))
                return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("FrozenWave/Cast/Dodge").Format(player.Name));
            var damage = (int)target.BlockDamage(player.CalculateMagicDamage(), DamageType.Magic);
            target.ReduceHealth(damage);
            target.Effects.Add(new EffectBase(LocalizationService.Get("effects").Get(lang).Get("Frozen/Name"), 2,
                new AllBonuses()
                {
                    Agility = -(int)(player.AllCharacteristics.GetFullAbilityPower() * 0.5f)
                }));
            return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("FrozenWave/Cast/Success").Format(player.Name, damage));
        }
    }
}
