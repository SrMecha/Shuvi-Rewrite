namespace Shuvi.Interfaces.Characteristics
{
    public interface IDynamicCharacteristics
    {
        public TCharacteristic Health { get; }
        public TCharacteristic Mana { get; }
        public TCharacteristic Energy { get; }
    }
}
