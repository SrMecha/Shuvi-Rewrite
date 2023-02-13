using Shuvi.Enums.Rating;
using Shuvi.Enums.Requirements;
using System.Runtime.CompilerServices;

namespace Shuvi.Classes.Extensions
{
    public static class EnumExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetString(this Rank target)
        {
            return target switch
            {
                Rank.E => nameof(Rank.E),
                Rank.D => nameof(Rank.D),
                Rank.C => nameof(Rank.C),
                Rank.B => nameof(Rank.B),
                Rank.A => nameof(Rank.A),
                Rank.S => nameof(Rank.S),
                Rank.SS => nameof(Rank.SS),
                Rank.SSS => nameof(Rank.SSS),
                _ => "Error"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this BaseRequirement target)
        {
            return target switch
            {
                BaseRequirement.Strength => "strength",
                BaseRequirement.Agility => "agility",
                BaseRequirement.Luck => "luck",
                BaseRequirement.Intellect => "intellect",
                BaseRequirement.Endurance => "endurance",
                BaseRequirement.Rank => "rank",
                _ => string.Empty
            };
        }
    }
}
