using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Interfaces.Characteristics
{
    public interface IUserCharacteristics : IEntityCharacteristics<IRestorableCharacteristic>
    {
        public IEnergy Energy { get; }

        public bool HaveEnergy(int amount);
    }
}
