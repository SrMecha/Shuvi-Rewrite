using Shuvi.Enums.Characteristic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Classes.Types.Characteristics.Static
{
    public class StaticCharacteristics : IStaticCharacteristics
    {
        public int Strength { get; protected set; } = 0;
        public int Agility { get; protected set; } = 0;
        public int Luck { get; protected set; } = 0;
        public int Intellect { get; protected set; } = 0;
        public int Endurance { get; protected set; } = 0;

        public StaticCharacteristics() { }
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
        public StaticCharacteristics(Dictionary<StaticCharacteristic, int> characteristics)
        {
            foreach (var (characteristic, amount) in characteristics)
                Add(characteristic, amount);
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
        public IEnumerator<(StaticCharacteristic, int)> GetEnumerator()
        {
            yield return (StaticCharacteristic.Strength, Strength);
            yield return (StaticCharacteristic.Agility, Agility);
            yield return (StaticCharacteristic.Luck, Luck);
            yield return (StaticCharacteristic.Intellect, Intellect);
            yield return (StaticCharacteristic.Endurance, Endurance);
        }
        public void Set(int strength, int agility, int luck, int intellect, int endurance)
        {
            Strength = strength;
            Agility = agility;
            Luck = luck;
            Intellect = intellect;
            Endurance = endurance;
        }
    }
}
