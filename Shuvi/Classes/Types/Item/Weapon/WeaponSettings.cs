using Shuvi.Classes.Data.Item;
using Shuvi.Interfaces.Items.Weapon;

namespace Shuvi.Classes.Types.Item.Weapon
{
    public class WeaponSettings : IWeaponSettings
    {
        public float DamageMultiplier { get; private set; }

        public WeaponSettings(WeaponSettingsData data)
        {
            DamageMultiplier = data.DamageMultiplier;
        }
    }
}
