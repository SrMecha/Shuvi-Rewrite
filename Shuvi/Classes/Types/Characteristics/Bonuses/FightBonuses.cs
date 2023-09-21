using Shuvi.Classes.Data.Bonuses;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Interfaces.Characteristics.Bonuses;

namespace Shuvi.Classes.Types.Characteristics.Bonuses
{
    public class FightBonuses : StaticCharacteristics, IFightBonuses
    {
        public float AttackDamage { get; set; } = 0;
        public float AbilityPower { get; set; } = 0;
        public float Armor { get; set; } = 0;
        public float MagicResistance { get; set; } = 0;
        public float CriticalStrikeChance { get; set; } = 0;
        public float DodgeChance { get; set; } = 0;
        public float StrikeChance { get; set; } = 0;

        public float AttackDamageMultiplier { get; set; } = 0f;
        public float AbilityPowerMultiplier { get; set; } = 0f;
        public float ArmorMultiplier { get; set; } = 0f;
        public float MagicResistanceMultiplier { get; set; } = 0f;
        public float CriticalStrikeDamageMultiplier { get; set; } = 0f;

        public FightBonuses() { }

        public FightBonuses(FightBonusesData data) : base(data)
        {
            AttackDamage += data.AttackDamage;
            AbilityPower += data.AbilityPower;
            Armor += data.Armor;
            MagicResistance += data.MagicResistance;
            CriticalStrikeChance += data.CriticalStrikeChance;
            DodgeChance += data.DodgeChance;

            AttackDamageMultiplier += data.AttackDamageMultiplier;
            AbilityPowerMultiplier += data.AbilityPowerMultiplier;
            ArmorMultiplier += data.ArmorMultiplier;
            MagicResistanceMultiplier += data.MagicResistanceMultiplier;
            CriticalStrikeDamageMultiplier += data.CriticalStrikeDamageMultiplier;
        }
        public void Add(IFightBonuses bonuses)
        {
            base.Add(bonuses);
            AttackDamage += bonuses.AttackDamage;
            AbilityPower += bonuses.AbilityPower;
            Armor += bonuses.Armor;
            MagicResistance += bonuses.MagicResistance;
            CriticalStrikeChance += bonuses.CriticalStrikeChance;
            DodgeChance += bonuses.DodgeChance;

            AttackDamageMultiplier += bonuses.AttackDamageMultiplier;
            AbilityPowerMultiplier += bonuses.AbilityPowerMultiplier;
            ArmorMultiplier += bonuses.ArmorMultiplier;
            MagicResistanceMultiplier += bonuses.MagicResistanceMultiplier;
            CriticalStrikeDamageMultiplier += bonuses.CriticalStrikeDamageMultiplier;
        }

        public IEnumerable<(string, float)> GetFightBonuses()
        {
            yield return ("AttackDamage", AttackDamage);
            yield return ("AbilityPower", AbilityPower);
            yield return ("Armor", Armor);
            yield return ("MagicResistance", MagicResistance);
            yield return ("CriticalStrikeChance", CriticalStrikeChance);
            yield return ("DodgeChance", DodgeChance);
            yield return ("StrikeChance", StrikeChance);

            yield return ("AttackDamageMultiplier", AttackDamageMultiplier);
            yield return ("AbilityPowerMultiplier", AbilityPowerMultiplier);
            yield return ("ArmorMultiplier", ArmorMultiplier);
            yield return ("MagicResistanceMultiplier", MagicResistanceMultiplier);
            yield return ("CriticalStrikeDamageMultiplier", CriticalStrikeDamageMultiplier);
        }

        public float GetFullAttackDamage(float bonus = 0, float multiplier = 0)
        {
            return GetAttackDamage() * (AttackDamageMultiplier + multiplier + 1) + AttackDamage + bonus;
        }

        public float GetFullAbilityPower(float bonus = 0, float multiplier = 0)
        {
            return GetAbilityPower() * (AbilityPowerMultiplier + multiplier + 1) + AbilityPower + bonus;
        }

        public float GetFullArmor(float bonus = 0, float multiplier = 0)
        {
            return GetArmor() * (ArmorMultiplier + multiplier + 1) + Armor + bonus;
        }

        public float GetFullMagicResistance(float bonus = 0, float multiplier = 0)
        {
            return GetMagicResistance() * (MagicResistanceMultiplier + multiplier + 1) + MagicResistance + bonus;
        }

        public float GetFullCriticalStrikeChance(float bonus = 0)
        {
            return GetCriticalStrikeChance() + CriticalStrikeChance + bonus;
        }

        public float GetFullCriticalStrikeDamageMultiplier(float bonus = 0)
        {
            return GetCriticalStrikeDamageMultiplier() + CriticalStrikeDamageMultiplier + bonus;
        }

        public float GetFullDodgeChance(float bonus = 0)
        {
            return GetDodgeChance() + DodgeChance + bonus;
        }

        public float GetFullStrikeChance(float bonus = 0)
        {
            return GetStrikeChance() + StrikeChance + bonus;
        }
    }
}
