namespace Shuvi.Interfaces.Characteristics.Dynamic
{
    public interface IDynamicCharacteristic
    {
        public int Max { get; }

        public void SetMax(int max);
    }
}
