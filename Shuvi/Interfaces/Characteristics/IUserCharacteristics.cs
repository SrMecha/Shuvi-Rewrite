using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Interfaces.Characteristics
{
    public interface IUserCharacteristics : IEntityCharacteristics<IRestorableCharacteristic>
    {
        public IRestorableCharacteristic Energy { get; }
    }
}
