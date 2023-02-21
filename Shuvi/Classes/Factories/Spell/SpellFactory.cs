using Shuvi.Classes.Types.Spell.SpellList;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Factories.Spell
{
    public static class SpellFactory
    {
        private static IReadOnlyDictionary<string, ISpell>? _spells;

        public static void SetDictionary(IReadOnlyDictionary<string, ISpell> spells)
        {
            _spells = spells;
        }

        public static ISpell GetSpell(string spellName)
        {
            if (_spells!.TryGetValue(spellName, out var result))
                return result.CreateCopy();
            return new VoidSpell();
        }
    }
}
