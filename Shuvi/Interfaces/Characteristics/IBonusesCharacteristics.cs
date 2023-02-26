using Shuvi.Enums.Characteristic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Interfaces.Characteristics
{
    public interface IBonusesCharacteristics : IStaticCharacteristics
    {
        public int Health { get; }
        public int Mana { get; }

        public void Add(IBonusesCharacteristics characteristics);
        public void Add(DynamicCharacteristic characteristic, int amount);
        public void Reduce(IBonusesCharacteristics characteristics);
        public void Reduce(DynamicCharacteristic characteristic, int amount);
    }
}
