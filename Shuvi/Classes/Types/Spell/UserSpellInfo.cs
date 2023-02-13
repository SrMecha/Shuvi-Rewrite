using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Types.Localization;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Types.Spell
{
    public class UserSpellInfo : IUserSpellInfo
    {
        private string? _spellName;

        public ILocalizedInfo Info { get; private set; }

        public UserSpellInfo(string? spellName)
        {
            _spellName = spellName;
            Info = new CachedLocalizedInfo("spells", spellName);
        }
        public void SetSpell(string? spellName)
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
