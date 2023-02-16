using Shuvi.Classes.Data.Item;
using Shuvi.Enums.Characteristic;
using Shuvi.Interfaces.Items;

namespace Shuvi.Classes.Types.Item
{
    public class PotionItem : SimpleItem, IPotionItem
    {
        public Dictionary<DynamicCharacteristic, int> PotionRecover { get; init; }

        public PotionItem(ItemData data) : base(data)
        {
            PotionRecover = data.PotionRecover ?? new();
        }
    }
}
