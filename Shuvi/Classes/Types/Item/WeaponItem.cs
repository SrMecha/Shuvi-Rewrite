using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Item.Weapon;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Items.Weapon;

namespace Shuvi.Classes.Types.Item
{
    public class WeaponItem : EquipmentItem, IWeaponItem
    {
        public IWeaponSettings WeaponSettings { get; init; }

        public WeaponItem(ItemData data) : base(data) 
        {
            WeaponSettings = new WeaponSettings(data.WeaponSettings ?? new());
        }
    }
}
