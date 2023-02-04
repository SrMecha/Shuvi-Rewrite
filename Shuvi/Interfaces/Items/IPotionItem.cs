using Shuvi.Enums.Characteristic;

namespace Shuvi.Interfaces.Items
{
    public interface IPotionItem : IItem
    {
        public Dictionary<DynamicCharacteristic, int> PotionRecover { get; init; }
    }
}
