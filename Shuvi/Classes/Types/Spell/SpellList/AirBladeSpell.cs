﻿using Shuvi.Classes.Extensions;
using Shuvi.Classes.Settings;
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
    public class AirBladeSpell : SpellBase
    {
        public override string SpellName { get; } = "AirBlade";
        public override ILocalizedInfo Info { get; } = new CachedLocalizedInfo("spells", "AirBlade");
        public override MagicType MagicType { get; } = MagicType.Wind;
        public override IBaseRequirements Requirements { get; } = new BaseRequirements();
        public override int Cost { get; } = 4;

        public override IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            if (target.IsDodged(player, FightSettings.LightAttackDodgeBonus))
                return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("AirBlade/Cast/Dodge").Format(player.Name));
            var damage = (int)target.BlockDamage(player.CalculateMagicDamage(), DamageType.Magic);
            target.ReduceHealth(damage);
            return new ActionResult(LocalizationService.Get("spells").Get(lang).Get("AirBlade/Cast/Success").Format(player.Name, damage));
        }
    }
}
