namespace Shuvi.Interfaces.Characteristics
{
    public interface IDynamicCharacteristic
    {
        public int Now { get; }
        public int Max { get; }

        public void Add(int amount);
        public void Reduce(int amount);
        public void SetMax(int max);
    }
}
