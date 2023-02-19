using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Enums.Characteristic;
using Shuvi.Interfaces.Characteristics;

namespace Shuvi.Classes.Types.Characteristics
{
    public class BonusesCharacteristics : StaticCharacteristics, IBonusesCharacteristics
    {
        public int Health { get; private set; }
        public int Mana { get; private set; }

        public BonusesCharacteristics(int strength = 0, int agility = 0, int luck = 0, int intellect = 0, int endurance = 0, int health = 0, int mana = 0)
        {
            Strength = strength;
            Agility = agility;
            Luck = luck;
            Intellect = intellect;
            Endurance = endurance;
            Health = health;
            Mana = mana;
        }
        public void Add(IBonusesCharacteristics characteristics)
        {
            Strength += characteristics.Strength;
            Agility += characteristics.Agility;
            Luck += characteristics.Luck;
            Intellect += characteristics.Intellect;
            Endurance += characteristics.Endurance;
            Health += characteristics.Health;
            Mana += characteristics.Mana;
        }
        public void Add(DynamicCharacteristic characteristic, int amount)
        {
            switch (characteristic)
            {
                case DynamicCharacteristic.Health:
                    Health += amount;
                    break;
                case DynamicCharacteristic.Mana:
                    Mana += amount;
                    break;
            }
        }
        public void Reduce(IBonusesCharacteristics characteristics)
        {
            Strength -= characteristics.Strength;
            Agility -= characteristics.Agility;
            Luck -= characteristics.Luck;
            Intellect -= characteristics.Intellect;
            Endurance -= characteristics.Endurance;
            Health -= characteristics.Health;
            Mana -= characteristics.Mana;
        }
        public void Reduce(DynamicCharacteristic characteristic, int amount)
        {
            switch (characteristic)
            {
                case DynamicCharacteristic.Health:
                    Health -= amount;
                    break;
                case DynamicCharacteristic.Mana:
                    Mana -= amount;
                    break;
            }
        }
    }
}
