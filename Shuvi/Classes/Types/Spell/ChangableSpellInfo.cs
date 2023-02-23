using Shuvi.Classes.Types.Localization;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Types.Spell
{
    public class ChangableSpellInfo : SpellInfo, IChangableSpellInfo
    {

        public ChangableSpellInfo(string? spellName) : base(spellName) { }
        public void SetSpell(string? spellName)
        {
            _spellName = spellName;
            Info = new CachedLocalizedInfo("spells", spellName);
        }
    }
}
