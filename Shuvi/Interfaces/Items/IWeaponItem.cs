using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Items.Weapon;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Items
{
    public interface IWeaponItem : IEquipmentItem
    {
        public IWeaponSettings WeaponSettings { get; init; }
    }
}
