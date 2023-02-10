using Shuvi.Enums.Characteristic;

namespace Shuvi.Interfaces.Characteristics.Static
{
    public interface IStaticCharacteristics
    {
        public int Strength { get; }
        public int Agility { get; }
        public int Luck { get; }
        public int Intellect { get; }
        public int Endurance { get; }

        public void Add(IStaticCharacteristics characteristics);
        public void Add(StaticCharacteristic characteristics, int amount);
        public void Reduce(IStaticCharacteristics characteristics);
        public void Reduce(StaticCharacteristic characteristics, int amount);
    }
}
