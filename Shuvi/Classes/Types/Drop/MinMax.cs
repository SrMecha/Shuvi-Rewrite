using Shuvi.Classes.Data.Drop;
using Shuvi.Interfaces.Drop;

namespace Shuvi.Classes.Types.Drop
{
    public sealed class MinMax : IMinMax
    {
        public int Max { get; private set; }
        public int Min { get; private set; }

        public MinMax(int max, int min)
        {
            Max = max;
            Min = min;
        }
        public MinMax(MinMaxData data)
        {
            Max = data.Max;
            Min = data.Min;
        }
    }
}
