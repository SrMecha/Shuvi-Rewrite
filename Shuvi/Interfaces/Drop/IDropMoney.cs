using Shuvi.Enums.Money;

namespace Shuvi.Interfaces.Drop
{
    public interface IDropMoney : IDictionary<MoneyType, IMinMax>
    {
        
    }
}
