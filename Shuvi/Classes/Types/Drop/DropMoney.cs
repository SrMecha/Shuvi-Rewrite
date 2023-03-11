using Shuvi.Classes.Data.Drop;
using Shuvi.Classes.Extensions;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Money;
using Shuvi.Interfaces.Drop;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Drop
{
    public sealed class DropMoney : Dictionary<MoneyType, IMinMax>, IDropMoney
    {
        public DropMoney(MoneyDropData data)
        {
            foreach (var (key, value) in data)
                Add(key, new MinMax(value));
        }
        public string GetChancesInfo(Language lang)
        {
            var result = new List<string>();
            foreach (var (type, amount) in this)
                result.Add($"{LocalizationService.Get("names").Get(lang).Get(type.GetLowerName())} " +
                    $"{amount.Min} - {amount.Max}{EmojiService.Get(type.GetLowerName())}");
            return string.Join("\n", result);
        }
    }
}
