namespace Shuvi.Interfaces.Characteristics.Dynamic
{
    public interface INotRestorableCharacteristic : IDynamicCharacteristic
    {
        public int Now { get; }

        public void Add(int amount);
        public void Reduce(int amount);
    }
}
