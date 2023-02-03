using Shuvi.Interfaces.Entities;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Items
{
    public interface IWeaponItem : IEquipmentItem
    {
        public IActionResult Attack(IEntity assaulter, IEntity defender);
    }
}
