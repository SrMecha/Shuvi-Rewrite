using Shuvi.Classes.Data.Actions;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Classes.Types.Effect;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Actions;
using Shuvi.Enums.Damage;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Effect;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Status;
using Shuvi.Services.StaticServices.Localization;
using Shuvi.Services.StaticServices.Math;

namespace Shuvi.Classes.Types.Combat
{
    public class CombatEntity : ICombatEntity
    {
        protected bool _isPreparingForDefense = false;
        protected bool _isPreparingForDodge = false;
        protected readonly Random _random = new();

        public string Name { get; protected set; } = string.Empty;
        public Rank Rank { get; protected set; } = Rank.E;
        public IEntityCharacteristics<INotRestorableCharacteristic> Characteristics { get; protected set; } = new EntityCharacteristics<INotRestorableCharacteristic>();
        public ISpell Spell { get; protected set; } = SpellFactory.GetSpell(string.Empty);
        public IAllBonuses EffectBonuses { get; protected set; } = new AllBonuses();
        public IFightBonuses AllCharacteristics { get; protected set; } = new FightBonuses();
        public IEffects Effects { get; protected set; } = new Effects();
        public virtual IFightActions Actions { get; protected set; } = new FightActions(new FightActionsData());

        public bool IsDead => Characteristics.Health.Now <= 0;

        public CombatEntity() { }

        public float BlockDamage(float damage, DamageType damageType = DamageType.Physic)
        {
            float outDamage;
            switch (damageType)
            {
                case DamageType.Physic:
                    outDamage = damage - AllCharacteristics.GetFullArmor(multiplier: _isPreparingForDefense ? FightSettings.PreparingDefenseMultiplier : 0f);
                    break;
                case DamageType.Magic:
                    outDamage = damage - AllCharacteristics.GetFullMagicResistance(multiplier: _isPreparingForDefense ? FightSettings.PreparingDefenseMultiplier : 0f);
                    break;
                default:
                    throw new ArgumentException($"Неопознанный тип урона {damageType.GetType().Name} в BlockDamage(float damage, DamageType damageType).");
            }
            return outDamage < 0 ? 0f : damage;
        }

        public float CalculateMagicDamage(float bonus = 0f, float multiplier = 0f)
        {
            var damage = AllCharacteristics.GetFullAbilityPower(
                bonus: bonus,
                multiplier: multiplier + Random.Shared.NextFloat(-FightSettings.DamageSpreadMultiplier, FightSettings.DamageSpreadMultiplier));
            return damage < 0 ? 0 : damage;
        }

        public float CalculateHeavyDamage(float bonus = 0f, float multiplier = 0f)
        {
            var damage = AllCharacteristics.GetFullAttackDamage(
                bonus: bonus,
                multiplier: multiplier + Random.Shared.NextFloat(-FightSettings.DamageSpreadMultiplier, FightSettings.DamageSpreadMultiplier) +
                FightSettings.HeavyAttackDamageMultiplier
                );
            return damage < 0 ? 0 : damage;
        }

        public float CalculateLightDamage(float bonus = 0f, float multiplier = 0f)
        {
            var damage = AllCharacteristics.GetFullAttackDamage(
                bonus: bonus,
                multiplier: multiplier + Random.Shared.NextFloat(-FightSettings.DamageSpreadMultiplier, FightSettings.DamageSpreadMultiplier) +
                FightSettings.LightAttackDamageMultiplier
                );
            return damage < 0 ? 0 : damage;
        }

        public IActionResult CastSpell(ICombatEntity target, Language lang)
        {
            return Spell.Cast(this, target, lang);
        }

        public IActionResult DealHeavyDamage(ICombatEntity target, Language lang)
        {
            if (target.IsDodged(this, FightSettings.HeavyAttackDodgeBonus))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Attack/HeavyMiss"), Name));
            int damage;
            if (IsCritical(target))
            {
                damage = (int)target.BlockDamage(MathService.CalculateMultiplier(CalculateHeavyDamage(), AllCharacteristics.GetFullCriticalStrikeDamageMultiplier(), true));
                target.ReduceHealth(damage);
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Attack/Critical"), Name, damage));
            }
            damage = (int)target.BlockDamage(CalculateHeavyDamage());
            target.ReduceHealth(damage);
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Attack/HeavyHit"), Name, damage));
        }

        public IActionResult DealLightDamage(ICombatEntity target, Language lang)
        {
            if (target.IsDodged(this, FightSettings.LightAttackDodgeBonus))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Attack/LightMiss"), Name));
            int damage;
            if (IsCritical(target))
            {
                damage = (int)target.BlockDamage(MathService.CalculateMultiplier(CalculateLightDamage(), AllCharacteristics.GetFullCriticalStrikeDamageMultiplier(), true));
                target.ReduceHealth(damage);
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Attack/Critical"), Name, damage));
            }
            damage = (int)target.BlockDamage(CalculateLightDamage());
            target.ReduceHealth(damage);
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Attack/LightHit"), Name, damage));
        }

        public bool IsDodged(ICombatEntity assaulter, float bonusDodgeChance = 0f)
        {
            return Random.Shared.NextDouble()
                <=
                AllCharacteristics.GetFullDodgeChance(bonus: (_isPreparingForDodge ? FightSettings.PreparingDodgeBonus : 0f) + bonusDodgeChance)
                - assaulter.AllCharacteristics.GetFullStrikeChance();
        }

        public bool IsCritical(ICombatEntity target)
        {
            return Random.Shared.NextDouble() <= AllCharacteristics.GetFullCriticalStrikeChance();
        }

        public IActionResult PreparingForDefense(ICombatEntity target, Language lang)
        {
            _isPreparingForDefense = true;
            if (IsInvisiblePreparing(target))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Preparing/Unknown"), Name));
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Preparing/Defense"), Name));
        }

        public IActionResult PreparingForDodge(ICombatEntity target, Language lang)
        {
            _isPreparingForDodge = true;
            if (IsInvisiblePreparing(target))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Preparing/Unknown"), Name));
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("Fight/Preparing/Dodge"), Name));
        }

        private bool IsInvisiblePreparing(ICombatEntity target)
        {
            return Random.Shared.NextFloat(0, 100) <= 15f + (AllCharacteristics.Intellect - target.AllCharacteristics.Intellect) / 2f;
        }

        public virtual IActionResult RandomAction(ICombatEntity target, Language lang)
        {
            return Actions.GetRandomAction() switch
            {
                FightAction.LightAttack => DealLightDamage(target, lang),
                FightAction.HeavyAttack => DealHeavyDamage(target, lang),
                FightAction.Dodge => PreparingForDodge(target, lang),
                FightAction.Defense => PreparingForDefense(target, lang),
                FightAction.Spell => Spell.CanCast(this) ? CastSpell(target, lang) : DealLightDamage(target, lang),
                _ => DealLightDamage(target, lang),
            };
        }

        public void ReduceHealth(int amount)
        {
            Characteristics.Health.Reduce(amount);
        }

        public void ReduceMana(int amount)
        {
            Characteristics.Mana.Reduce(amount);
        }

        public void RestoreHealth(int amount)
        {
            Characteristics.Health.Add(amount);
        }

        public void RestoreMana(int amount)
        {
            Characteristics.Mana.Add(amount);
        }

        public virtual IResultStorage Update(Language lang)
        {
            _isPreparingForDefense = false;
            _isPreparingForDodge = false;
            EffectBonuses = Effects.UpdateAll(this);
            AllCharacteristics = new FightBonuses();
            AllCharacteristics.Add(EffectBonuses);
            AllCharacteristics.Add(Characteristics);
            var result = new ResultStorage();
            result.Add(Spell.Update(lang));
            return result;
        }
    }
}
