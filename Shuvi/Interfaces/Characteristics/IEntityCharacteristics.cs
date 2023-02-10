using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Interfaces.Characteristics
{
    public interface IEntityCharacteristics<TDynamic> : IStaticCharacteristics
        where TDynamic : IDynamicCharacteristic
    {
        public TDynamic Health { get; }
        public TDynamic Mana { get; }
    }
}
