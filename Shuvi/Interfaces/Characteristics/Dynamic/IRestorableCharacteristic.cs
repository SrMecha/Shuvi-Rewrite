namespace Shuvi.Interfaces.Characteristics.Dynamic
{
    public interface IRestorableCharacteristic : IDynamicCharacteristic
    {
        public void Add(int amount);
        public void Reduce(int amount);
        public int GetCurrent();
        public int GetRemainingRegenTime();
    }
}
