using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Characteristics.Static;
using Shuvi.Classes.Types.Requirements;
using Shuvi.Interfaces.Characteristics.Static;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Requirements;

namespace Shuvi.Classes.Types.Item
{
    public class EquipmentItem : SimpleItem, IEquipmentItem
    {
        public IBaseRequirements Requirements { get; init; }
        public IStaticCharacteristics Bonuses { get; init; }

        public EquipmentItem(ItemData data) : base(data)
        {
            Requirements = new BaseRequirements(data.Needs ?? new());
            Bonuses = new StaticCharacteristics(data.Bonuses ?? new());
        }
    }
}
