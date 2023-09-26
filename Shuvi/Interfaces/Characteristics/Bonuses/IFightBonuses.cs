using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Interfaces.Characteristics.Bonuses
{
    public interface IFightBonuses : IStaticCharacteristics
    {
        public float AttackDamage { get; }
        public float AbilityPower { get; }
        public float Armor { get; }
        public float MagicResistance { get; }
        public float CriticalStrikeChance { get; }
        public float DodgeChance { get; }
        public float StrikeChance { get; }

        public float AttackDamageMultiplier { get; }
        public float AbilityPowerMultiplier { get; }
        public float ArmorMultiplier { get; }
        public float MagicResistanceMultiplier { get; }
        public float CriticalStrikeDamageMultiplier { get; }

        public float GetFullAttackDamage(float bonus = 0f, float multiplier = 0f);
        public float GetFullAbilityPower(float bonus = 0f, float multiplier = 0f);
        public float GetFullArmor(float bonus = 0f, float multiplier = 0f);
        public float GetFullMagicResistance(float bonus = 0f, float multiplier = 0f);
        public float GetFullCriticalStrikeChance(float bonus = 0f);
        public float GetFullCriticalStrikeDamageMultiplier(float bonus = 0f);
        public float GetFullDodgeChance(float bonus = 0f);
        public float GetFullStrikeChance(float bonus = 0f);
        public void Add(IFightBonuses bonuses);
        public IEnumerable<(string, float)> GetFightBonuses();
    }
}
