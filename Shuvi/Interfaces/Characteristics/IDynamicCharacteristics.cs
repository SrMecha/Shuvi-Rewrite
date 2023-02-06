namespace Shuvi.Interfaces.Characteristics
{
    public interface IDynamicCharacteristics
    {
        public IDynamicCharacteristic Health { get; }
        public IDynamicCharacteristic Mana { get; }
        public IDynamicCharacteristic Energy { get; }
    }
}
