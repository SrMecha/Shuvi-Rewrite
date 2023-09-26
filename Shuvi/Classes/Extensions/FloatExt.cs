using System.Runtime.CompilerServices;

namespace Shuvi.Classes.Extensions
{
    public static class FloatExt
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string WithBonus(this float target, float bonus)
        {
            return bonus == 0 ? target.ToString() : bonus > 0 ? $"{target} | +{bonus}" : $"{target} | {bonus}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string WithBonusPercent(this float target, float bonus)
        {
            return bonus == 0 ? $"{target}%" : bonus > 0 ? $"{target}% | +{bonus}%" : $"{target}% | {bonus}%";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string WithSign(this float target, bool hideZero = false)
        {
            return target == 0 ? hideZero ? string.Empty : target.ToString() : target > 0 ? $"+{target}" : target.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string WithBonuses(this float target, float bonus, float multiplier)
        {
            return bonus == 0 && multiplier == 0
                ?
                target.ToString()
                :
                $"`({(bonus == 0 ? string.Empty : bonus.WithSign())}{(bonus != 0 && multiplier != 0 ? " | " : string.Empty)}" +
                $"{(multiplier == 0 ? string.Empty : $"{multiplier.WithSign()}%")})`";
        }
    }
}
