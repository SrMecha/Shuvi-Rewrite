using System.Runtime.CompilerServices;

namespace Shuvi.Classes.Extensions
{
    public static class StringExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this string target, object? arg0)
        {
            return string.Format(target, arg0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this string target, object? arg0, object? arg1)
        {
            return string.Format(target, arg0, arg1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this string target, object? arg0, object? arg1, object? arg2)
        {
            return string.Format(target, arg0, arg1, arg2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this string target, object? arg0, object? arg1, object? arg2, object? arg3)
        {
            return string.Format(target, arg0, arg1, arg2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this string target, object? arg0, object? arg1, object? arg2, object? arg3, object? arg4)
        {
            return string.Format(target, arg0, arg1, arg2, arg3, arg4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this string target, object? arg0, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
        {
            return string.Format(target, arg0, arg1, arg2, arg3, arg4, arg5);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this string target, object? arg0, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
        {
            return string.Format(target, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Multiple(this string original, byte repeatCount)
        {
            if (repeatCount < 1) return string.Empty;
            return string.Create(original.Length * ++repeatCount, original, (span, value) =>
            {
                for (var i = 0; i < (span.Length - value.Length); i += value.Length)
                    for (var j = 0; j < value.Length; j++)
                        span[i + j] = value[j];
            });
        }
    }
}
