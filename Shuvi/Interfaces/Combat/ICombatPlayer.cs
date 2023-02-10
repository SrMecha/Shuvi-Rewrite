using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Combat
{
    public interface ICombatPlayer
    {
        public ISkill Skill { get; }
        public IUserInventory Inventory { get; }
        public IActionResult UseSkill(ICombatEntity target);
    }
}
