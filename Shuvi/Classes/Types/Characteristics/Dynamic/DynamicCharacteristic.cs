using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Classes.Types.Characteristics.Dynamic
{
    public class DynamicCharacteristic : IDynamicCharacteristic
    {
        public int Max { get; protected set; }

        
        public void SetMax(int max)
        {
            Max = max;
        }
    }
}
