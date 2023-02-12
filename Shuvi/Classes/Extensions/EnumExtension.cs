using Shuvi.Enums.Rating;
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
    }
}
