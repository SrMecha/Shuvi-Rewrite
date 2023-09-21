using Shuvi.Classes.Data.Bonuses;
using Shuvi.Classes.Settings;
using Shuvi.Enums.Characteristic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Classes.Types.Characteristics.Static
{
    public class StaticCharacteristics : IStaticCharacteristics
    {
        public int Strength { get; set; } = 0;
        public int Agility { get; set; } = 0;
        public int Luck { get; set; } = 0;
        public int Intellect { get; set; } = 0;
        public int Endurance { get; set; } = 0;

        public StaticCharacteristics() { }

        public StaticCharacteristics(StaticBonusesData data)
        {
            Strength = data.Strength;
            Agility = data.Agility;
            Luck = data.Luck;
            Intellect = data.Intellect;
            Endurance = data.Endurance;
        }
        public StaticCharacteristics(IStaticCharacteristics characteristics)
        {
            Strength = characteristics.Strength;
            Agility = characteristics.Agility;
            Luck = characteristics.Luck;
            Intellect = characteristics.Intellect;
            Endurance = characteristics.Endurance;
        }

        public StaticCharacteristics(Dictionary<StaticCharacteristic, int> characteristics)
        {
            foreach (var (characteristic, amount) in characteristics)
                Add(characteristic, amount);
        }

        public float GetAbilityPower()
        {
            return Intellect * 0.6f + 1;
        }

        public float GetArmor()
        {
            return Endurance * 0.4f;
        }

        public float GetAttackDamage()
        {
            return Strength * 0.8f + 1;
        }

        public float GetCriticalStrikeChance()
        {
            return (FightSettings.StandartCriticalChance * 100 + (float)Math.Sqrt((Luck - 1) * (6.685f + 0.001675f * (Luck - 1)))) * 0.01f;
        }

        public float GetCriticalStrikeDamageMultiplier()
        {
            return FightSettings.StandartCriticalDamageMultiplier;
        }

        public float GetMagicResistance()
        {
            return Intellect * 0.4f;
        }

        public void Add(IStaticCharacteristics characteristics)
        {
            Strength += characteristics.Strength;
            Agility += characteristics.Agility;
            Luck += characteristics.Luck;
            Intellect += characteristics.Intellect;
            Endurance += characteristics.Endurance;
        }
        public void Add(StaticCharacteristic characteristic, int amount)
        {
            switch (characteristic)
            {
                case StaticCharacteristic.Strength:
                    Strength += amount;
                    break;
                case StaticCharacteristic.Agility:
                    Agility += amount;
                    break;
                case StaticCharacteristic.Luck:
                    Luck += amount;
                    break;
                case StaticCharacteristic.Intellect:
                    Intellect += amount;
                    break;
                case StaticCharacteristic.Endurance:
                    Endurance += amount;
                    break;
            }
        }
        public void Reduce(IStaticCharacteristics characteristics)
        {
            Strength -= characteristics.Strength;
            Agility -= characteristics.Agility;
            Luck -= characteristics.Luck;
            Intellect -= characteristics.Intellect;
            Endurance -= characteristics.Endurance;
        }
        public void Reduce(StaticCharacteristic characteristic, int amount)
        {
            switch (characteristic)
            {
                case StaticCharacteristic.Strength:
                    Strength -= amount;
                    break;
                case StaticCharacteristic.Agility:
                    Agility -= amount;
                    break;
                case StaticCharacteristic.Luck:
                    Luck -= amount;
                    break;
                case StaticCharacteristic.Intellect:
                    Intellect -= amount;
                    break;
                case StaticCharacteristic.Endurance:
                    Endurance -= amount;
                    break;
            }
        }
        public IEnumerable<(string, int)> GetStaticCharacteristics()
        {
            yield return ("Strength", Strength);
            yield return ("Agility", Agility);
            yield return ("Luck", Luck);
            yield return ("Intellect", Intellect);
            yield return ("Endurance", Endurance);
        }
        public void Set(int strength, int agility, int luck, int intellect, int endurance)
        {
            Strength = strength;
            Agility = agility;
            Luck = luck;
            Intellect = intellect;
            Endurance = endurance;
        }

        public float GetDodgeChance()
        {
            return Agility * 0.5f + FightSettings.StandartDodgeChance;
        }

        public float GetStrikeChance()
        {
            return Agility * 0.5f;
        }
    }
}
