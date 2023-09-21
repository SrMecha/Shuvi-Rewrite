namespace Shuvi.Classes.Extensions
{
    public static class RandomExt
    {
        public static float NextFloat(this Random random, float minValue, float maxValue) 
        {
            return ((float)random.NextDouble() * (maxValue - minValue)) + minValue;
        }
    }
}
