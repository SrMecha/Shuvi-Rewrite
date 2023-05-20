using Shuvi.Interfaces.Combat;

namespace Shuvi.Classes.Types.Spell.SpellList
{
    public class VoidSpell : SpellBase
    {
        public override string SpellName { get; } = "VoidSpell";

        public override bool CanCast(ICombatEntity player)
        {
            return false;
        }

        public override bool HaveSpell()
        {
            return false;
        }
    }
}
