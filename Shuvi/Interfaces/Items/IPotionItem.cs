using Shuvi.Interfaces.Characteristics;

namespace Shuvi.Interfaces.Items
{
    public interface IPotionItem : IItem
    {
        public IDynamicCharacteristics PotionRecover { get; init; }
    }
}
