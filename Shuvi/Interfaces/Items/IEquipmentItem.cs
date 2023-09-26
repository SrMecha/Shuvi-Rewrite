using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Requirements;

namespace Shuvi.Interfaces.Items
{
    public interface IEquipmentItem : IItem
    {
        public IBaseRequirements Requirements { get; init; }
        public IAllBonuses Bonuses { get; init; }
    }
}
