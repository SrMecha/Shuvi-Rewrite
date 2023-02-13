using Shuvi.Interfaces.Combat;

namespace Shuvi.Classes.Types.Spell.SpellList
{
    public class VoidSpell : SpellBase
    {
        public VoidSpell() { }
        public override bool CanCast(ICombatEntity player)
        {
            return false;
        }
    }
}
