using Shuvi.Enums.Characteristic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Classes.Types.Characteristics.Static
{
    public class StaticCharacteristics : IStaticCharacteristics
    {
        public int Strength { get; private set; } = 1;
        public int Agility { get; private set; } = 1;
        public int Luck { get; private set; } = 1;
        public int Intellect { get; private set; } = 1;
        public int Endurance { get; private set; } = 1;

        public StaticCharacteristics(int strength, int agility, int luck, int intellect, int endurance)
        {
            Strength = strength;
            Agility = agility;
            Luck = luck;
            Intellect = intellect;
            Endurance = endurance;
        }
        public StaticCharacteristics(IStaticCharacteristics characteristics)
        {
            Strength = characteristics.Strength;
            Agility = characteristics.Agility;
            Luck = characteristics.Luck;
            Intellect = characteristics.Intellect;
            Endurance = characteristics.Endurance;
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
    }
}
