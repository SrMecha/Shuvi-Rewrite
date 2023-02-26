namespace Shuvi.Interfaces.Rate
{
    public interface IRandomWithChance<T> where T : notnull
    {
        public T GetRandom();
    }
}
