namespace Shuvi.Interfaces.Characteristics.Dynamic
{
    public interface IRestorableCharacteristic : IDynamicCharacteristic
    {
        public int GetCurrent();

    }
}
