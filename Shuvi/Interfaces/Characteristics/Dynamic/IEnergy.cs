namespace Shuvi.Interfaces.Characteristics.Dynamic
{
    public interface IEnergy : IRestorableCharacteristic
    {
        public int GetMax(int endurance);
    }
}
