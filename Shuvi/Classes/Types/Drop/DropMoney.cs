using Shuvi.Classes.Data.Drop;
using Shuvi.Enums.Money;
using Shuvi.Interfaces.Drop;

namespace Shuvi.Classes.Types.Drop
{
    public sealed class DropMoney : Dictionary<MoneyType, IMinMax>, IDropMoney
    {
        public DropMoney(MoneyDropData data)
        {
            foreach (var (key, value) in data)
                Add(key, new MinMax(value));
        }
    }
}
