using Shuvi.Classes.Types.Spell.SpellList;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Factories.Spell
{
    public static class SpellFactory
    {
        private static Dictionary<string, ISpell> _spells = new()
        {
            { "void", new VoidSpell() }
        };

        public static ISpell GetSpell(string spellName)
        {
            return _spells.GetValueOrDefault(spellName, new VoidSpell());
        }
    }
}
