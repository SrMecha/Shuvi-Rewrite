using Shuvi.Enums.Localization;
using Shuvi.Enums.Money;

namespace Shuvi.Interfaces.Drop
{
    public interface IDropMoney : IDictionary<MoneyType, IMinMax>
    {
        public string GetChancesInfo(Language lang);
    }
}
