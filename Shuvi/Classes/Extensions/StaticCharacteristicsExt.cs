using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Characteristics.Static;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Extensions
{
    public static class StaticCharacteristicsExt
    {
        public static string GetBonusesInfo(this IStaticCharacteristics characteristics, Language lang)
        {
            var result = new List<string>();
            foreach (var (characteristic, amount) in characteristics)
                if (amount != 0)
                    result.Add($"{LocalizationService.Get("names").Get(lang).Get(characteristic.GetLowerName())} {(amount < 0 ? amount : $"+{amount}")}");
            if (result.Count < 1)
                return LocalizationService.Get("names").Get(lang).Get("notHave");
            return string.Join("\n", result);
        }
    }
}
