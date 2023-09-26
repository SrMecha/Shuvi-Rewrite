namespace Shuvi.Services.StaticServices.Math
{
    public static class MathService
    {
        public static float CalculateMultiplier(float baseValue, float multiplier, bool withBaseValue)
        {
            return baseValue * (withBaseValue ? 1 : 0 + (multiplier * 0.01f));
        }
    }
}
