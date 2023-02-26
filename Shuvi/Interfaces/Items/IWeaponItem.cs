using Shuvi.Interfaces.Items.Weapon;

namespace Shuvi.Interfaces.Items
{
    public interface IWeaponItem : IEquipmentItem
    {
        public IWeaponSettings WeaponSettings { get; init; }
    }
}
