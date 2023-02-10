using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
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
        public bool EffectBonuses { get; }
        public bool Effects { get; }

        public IActionResult CastSpell(ICombatEntity target);
        public IActionResult DealLightDamage(ICombatEntity target);
        public IActionResult DealHeavyDamage(ICombatEntity target);
        public float CalculateLightDamage();
        public float CalculateHeavyDamage();
        public IActionResult PreparingForDefense(ICombatEntity target);
        public IActionResult PreparingForDodge(ICombatEntity target);
        public int BlockDamage(float damage);
        public bool IsDodged(ICombatEntity assaulter, int hitBonusChance);
        public void RestoreHealth(int amount);
        public void ReduceHealth(int amount);
        public void RestoreMana(int amount);
        public void ReduceMana(int amount);
        public void Update();
    }
}
