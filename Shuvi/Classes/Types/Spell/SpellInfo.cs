using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Types.Localization;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Types.Spell
{
    public class SpellInfo : ISpellInfo
    {
        protected string? _spellName;

        public ILocalizedInfo Info { get; protected set; }

        public SpellInfo(string? spellName)
        {
            _spellName = spellName;
            Info = new CachedLocalizedInfo("spells", spellName);
        }
        public ISpell GetSpell()
        {
            return SpellFactory.GetSpell(_spellName ?? string.Empty);
        }
    }
}
