namespace Shuvi.Classes.Extensions
{
    public static class IntExt
    {
        public static string WithBonus(this int target, int bonus)
        {
            return bonus == 0 ? target.ToString() : bonus > 0 ? $"{target} | +{bonus}" : $"{target} | {bonus}";
        }
        public static string WithSign(this int target, bool hideZero = false)
        {
            return target == 0 ? hideZero ? string.Empty : target.ToString() : target > 0 ? $"+{target}" : target.ToString();
        }
        public static bool IsInfinity(this int target)
        {
            return target == -1;
        }
    }
}
