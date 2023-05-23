using Shuvi.Classes.Settings;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Characteristics.Static;

namespace Shuvi.Classes.Types.Characteristics.Dynamic
{
    public class Energy : RestorableCharacteristic, IEnergy
    {
        protected IStaticCharacteristics _characteristics;
        public override int Max { get { return UserSettings.StandartEnergy + (_characteristics.Endurance / UserSettings.EndurancePerEnergy); } }

        public Energy(IStaticCharacteristics characteristics, long regenTime)
            : base(1, regenTime, UserSettings.EnergyPointRegenTime)
        {
            _characteristics = characteristics;
        }
    }
}
