using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Classes.Types.Characteristics
{
    public class UserCharacteristics : EntityCharacteristics<IRestorableCharacteristic>, IUserCharacteristics
    {
        public IEnergy Energy { get; private set; }

        public UserCharacteristics(IStaticCharacteristics characteristics, IRestorableCharacteristic health,
            IRestorableCharacteristic mana, long energyRegenTime)
            : base(characteristics, health, mana)
        {
            Energy = new Energy(this, energyRegenTime);
        }
        public bool HaveEnergy(int amount)
        {
            return Energy.GetCurrent() >= amount;
        }
    }
}
