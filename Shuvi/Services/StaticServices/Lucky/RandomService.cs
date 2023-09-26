namespace Shuvi.Services.StaticServices.Lucky
{
    public static class RandomService
    {
        public static bool IsLucky(float chance)
        {
            return Random.Shared.NextDouble() * 100 <= chance;
        }
    }
}
