using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Effect;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Combat
{
    public interface ICombatEntity
    {
        public string Name { get; }
        public Rank Rank { get; }
        public IEntityCharacteristics<INotRestorableCharacteristic> Characteristics { get; }
        public ISpell Spell { get; }
        public bool IsDead { get; }
        public IBonusesCharacteristics EffectBonuses { get; }
        public IEffects Effects { get; }
        public IFightActions Actions { get; }

        public IActionResult RandomAction(ICombatEntity target, Language lang);
        public IActionResult PreparingForDefense(ICombatEntity target, Language lang);
        public IActionResult PreparingForDodge(ICombatEntity target, Language lang);
        public IActionResult CastSpell(ICombatEntity target, Language lang);
        public IActionResult DealLightDamage(ICombatEntity target, Language lang);
        public IActionResult DealHeavyDamage(ICombatEntity target, Language lang);
        public float CalculateLightDamage();
        public float CalculateHeavyDamage();
        public int BlockDamage(float damage);
        public bool IsDodged(ICombatEntity assaulter, int hitBonusChance);
        public bool IsCritical(ICombatEntity target);
        public void RestoreHealth(int amount);
        public void ReduceHealth(int amount);
        public void RestoreMana(int amount);
        public void ReduceMana(int amount);
        public IResultStorage Update(Language lang);
    }
}
