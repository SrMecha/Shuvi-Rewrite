using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.Combat
{
    public class CombatPlayer : CombatEntity, ICombatPlayer
    {
        public ISkill Skill { get; private set; }
        public IUserInventory Inventory { get; private set; }

        public CombatPlayer(IDatabaseUser user) 
        { 

        }
        public IActionResult UseSkill(ICombatEntity target)
        {
            throw new NotImplementedException();
        }
    }
}
