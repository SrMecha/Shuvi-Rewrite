using Shuvi.Interfaces.Characteristics.Static;
using Shuvi.Interfaces.Requirements;

namespace Shuvi.Interfaces.Items
{
    public interface IEquipmentItem : IItem
    {
        public IBaseRequirements Requirements { get; init; }
        public IStaticCharacteristics Bonuses { get; init; }
    }
}
