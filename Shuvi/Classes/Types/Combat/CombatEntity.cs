using Shuvi.Classes.Data.Actions;
using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Effect;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Actions;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Effect;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Status;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Combat
{
    public class CombatEntity : ICombatEntity
    {
        protected bool _isPreparingForDefense = false;
        protected bool _isPreparingForDodge = false;
        protected readonly Random _random = new();

        public string Name { get; protected set; }
        public Rank Rank { get; protected set; }
        public IEntityCharacteristics<INotRestorableCharacteristic> Characteristics { get; protected set; }
        public ISpell Spell { get; protected set; }
        public IBonusesCharacteristics EffectBonuses { get; protected set; }
        public IEffects Effects { get; protected set; }
        public virtual IFightActions Actions { get; protected set; }

        public bool IsDead => Characteristics.Health.Now <= 0;

        public CombatEntity()
        {
            Name = string.Empty;
            Rank = Rank.E;
            Characteristics = new EntityCharacteristics<INotRestorableCharacteristic>(1, 1, 1, 1, 1,
                new NotRestorableCharacteristic(1, 1), new NotRestorableCharacteristic(1, 1));
            Spell = SpellFactory.GetSpell(string.Empty);
            EffectBonuses = new BonusesCharacteristics();
            Effects = new Effects();
            Actions = new FightActions(new FightActionsData());
        }
        public int BlockDamage(float damage)
        {
            var blockedDamageBonus = 0.0f;
            if (_isPreparingForDefense)
                blockedDamageBonus = (Characteristics.Endurance + EffectBonuses.Endurance) / 2.0f;
            var outDamage = (int)(damage - ((Characteristics.Endurance + EffectBonuses.Endurance + blockedDamageBonus) / 3.0f) + 0.5f);
            // 0.5f для округления float в большую сторону.
            if (outDamage < 0)
                outDamage = 0;
            return outDamage;
        }
        public float CalculateHeavyDamage()
        {
            return (Characteristics.Strength + EffectBonuses.Strength) * _random.Next(120, 131) / 100.0f + FightSettings.DamageBonus;
        }
        public float CalculateLightDamage()
        {
            return (Characteristics.Strength + EffectBonuses.Strength) * _random.Next(70, 81) / 100.0f + FightSettings.DamageBonus;
        }
        public IActionResult CastSpell(ICombatEntity target, Language lang)
        {
            return Spell.Cast(this, target, lang);
        }
        public IActionResult DealHeavyDamage(ICombatEntity target, Language lang)
        {
            if (target.IsDodged(this, 0))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/attack/heavyMiss"), Name));
            int damage;
            if (IsCritical(target))
            {
                damage = target.BlockDamage(CalculateHeavyDamage()) * 2;
                target.ReduceHealth(damage);
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/attack/critical"), Name, damage));
            }
            damage = target.BlockDamage(CalculateHeavyDamage());
            target.ReduceHealth(damage);
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/attack/heavyHit"), Name, damage));
        }
        public IActionResult DealLightDamage(ICombatEntity target, Language lang)
        {
            if (target.IsDodged(this, 30))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/attack/lightMiss"), Name));
            int damage;
            if (IsCritical(target))
            {
                damage = target.BlockDamage(CalculateLightDamage()) * 2;
                target.ReduceHealth(damage);
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/attack/critical"), Name, damage));
            }
            damage = target.BlockDamage(CalculateLightDamage());
            target.ReduceHealth(damage);
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/attack/lightHit"), Name, damage));
        }
        public bool IsDodged(ICombatEntity assaulter, int hitBonusChance)
        {
            var dodgeChance = ((_isPreparingForDodge ? FightSettings.PreparingDodgeBonus : FightSettings.StandartDodgeBonus) - hitBonusChance +
                (Characteristics.Agility + EffectBonuses.Agility - (assaulter.Characteristics.Agility + assaulter.EffectBonuses.Agility))) / 2;
            return _random.Next(0, 101) <= dodgeChance;
        }
        public bool IsCritical(ICombatEntity target)
        {
            var criticalChance = (FightSettings.StandartCriticalBonus +
                Characteristics.Luck + EffectBonuses.Luck - (target.Characteristics.Luck + target.EffectBonuses.Luck)) / 2;
            return _random.Next(0, 101) <= criticalChance;
        }
        public IActionResult PreparingForDefense(ICombatEntity target, Language lang)
        {
            _isPreparingForDefense = true;
            if (IsInvisiblePreparing(target))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/preparing/unknown"), Name));
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/preparing/defense"), Name));
        }
        public IActionResult PreparingForDodge(ICombatEntity target, Language lang)
        {
            _isPreparingForDodge = true;
            if (IsInvisiblePreparing(target))
                return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/preparing/unknown"), Name));
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("fight/preparing/dodge"), Name));
        }
        private bool IsInvisiblePreparing(ICombatEntity target)
        {
            var invisiblePreparingChance = (30 +
                Characteristics.Intellect + EffectBonuses.Intellect - (target.Characteristics.Intellect + target.EffectBonuses.Intellect)) / 2;
            return _random.Next(0, 101) <= invisiblePreparingChance;
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
            var result = new ResultStorage();
            result.Add(Spell.Update(lang));
            return result;
        }
    }
}
