using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Classes.Types.Characteristics.Dynamic
{
    public class DynamicCharacteristic : IDynamicCharacteristic
    {
        public int Max { get; protected set; }

        public DynamicCharacteristic(int max)
        {
            Max = max;
        }
        public void SetMax(int max)
        {
            Max = max;
        }
    }
}
