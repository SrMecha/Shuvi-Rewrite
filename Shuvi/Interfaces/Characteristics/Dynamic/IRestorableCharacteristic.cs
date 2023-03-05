namespace Shuvi.Interfaces.Characteristics.Dynamic
{
    public interface IRestorableCharacteristic : IDynamicCharacteristic
    {
        public long RegenTime { get; }

        public void Add(int amount);
        public void Reduce(int amount);
        public int GetCurrent();
        public int GetRemainingRegenTime();
    }
}
