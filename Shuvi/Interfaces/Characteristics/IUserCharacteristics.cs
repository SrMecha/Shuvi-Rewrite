using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Interfaces.Characteristics
{
    public interface IUserCharacteristics : IEntityCharacteristics<IRestorableCharacteristic>
    {
        public IRestorableCharacteristic Energy { get; }
    }
}
