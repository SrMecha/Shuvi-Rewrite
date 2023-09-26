using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Classes.Types.Characteristics
{
    public class UserCharacteristics : EntityCharacteristics<IRestorableCharacteristic>, IUserCharacteristics
    {
        public IEnergy Energy { get; set; } = default!;

        public bool HaveEnergy(int amount)
        {
            return Energy.GetCurrent() >= amount;
        }
    }
}
