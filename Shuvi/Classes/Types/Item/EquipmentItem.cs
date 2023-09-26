using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Classes.Types.Requirements;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Requirements;

namespace Shuvi.Classes.Types.Item
{
    public class EquipmentItem : SimpleItem, IEquipmentItem
    {
        public IBaseRequirements Requirements { get; init; }
        public IAllBonuses Bonuses { get; init; }

        public EquipmentItem(ItemData data) : base(data)
        {
            Requirements = new BaseRequirements(data.Needs ?? new());
            Bonuses = new AllBonuses(data.Bonuses ?? new());
        }
    }
}
