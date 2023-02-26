using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Combat
{
    public interface ICombatPlayer
    {
        public ISkill Skill { get; }
        public IDropInventory Inventory { get; }
        public IActionResult UseSkill(ICombatEntity target, Language lang);
    }
}
