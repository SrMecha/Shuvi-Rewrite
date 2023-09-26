using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Interfaces.Characteristics.Bonuses
{
    public interface ICharacteristicBonuses : IStaticCharacteristics, IDynamicBonuses
    {
        public void Add(ICharacteristicBonuses bonuses);
    }
}
