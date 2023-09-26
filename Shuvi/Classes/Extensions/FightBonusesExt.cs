using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Characteristics.Static;
using Shuvi.Services.StaticServices.Localization;
using System.Reflection.PortableExecutable;

namespace Shuvi.Classes.Extensions
{
    public static class FightBonusesExt
    {
        private static readonly List<string> _percentageBonuses = new() 
        {
            "CriticalStrikeChance",
            "DodgeChance",
            "StrikeChance",
            "AttackDamageMultiplier",
            "AbilityPowerMultiplier",
            "ArmorMultiplier",
            "MagicResistanceMultiplier",
            "CriticalStrikeDamageMultiplier",
            "DodgeChanceMultiplier",
            "StrikeChanceMultiplier"
        };

        private static bool IsPercentageBonus(this string bonus)
        {
            return _percentageBonuses.Contains(bonus);
        }

        public static string GetBonusesInfo(this IFightBonuses bonuses, Language lang)
        {
            var result = new List<string>();
            foreach (var (characteristic, amount) in bonuses.GetStaticCharacteristics())
                if (amount != 0)
                    result.Add($"{LocalizationService.Get("names").Get(lang).Get(characteristic)} " +
                        $"{amount.WithSign()}{(characteristic.IsPercentageBonus() ? "%" : string.Empty)}");

            foreach (var (bonus, amount) in bonuses.GetFightBonuses())
                if (amount != 0)
                    result.Add($"{LocalizationService.Get("names").Get(lang).Get(bonus)} {amount.WithSign()}" +
                        $"{(bonus.IsPercentageBonus() ? "%" : string.Empty)}");
            if (result.Count < 1)
                return LocalizationService.Get("names").Get(lang).Get("NotHave");
            return string.Join("\n", result);
        }
    }
}
