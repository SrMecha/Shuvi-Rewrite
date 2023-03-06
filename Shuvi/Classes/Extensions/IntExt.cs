namespace Shuvi.Classes.Extensions
{
    public static class IntExt
    {
        public static string WithBonus(this int target, int bonus)
        {
            return bonus == 0 ? target.ToString() : bonus > 0 ? $"{target} | +{bonus}" : $"{target} | {bonus}";
        }
        public static bool IsInfinity(this int target)
        {
            return target == -1;
        }
    }
}
