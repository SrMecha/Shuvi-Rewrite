using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Magic;
using Shuvi.Enums.Magic;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Types.Spell
{
    public class ChangableSpellInfo : SpellInfo, IChangableSpellInfo
    {

        public ChangableSpellInfo(string? spellName, MagicType magicType = MagicType.None) : base(spellName, magicType) { }
        public void SetSpell(string? spellName)
        {
            _spellName = spellName;
            Info = new CachedLocalizedInfo("spells", spellName);
            MagicInfo = new MagicInfo(SpellFactory.GetSpell(spellName ?? string.Empty).MagicType);
        }
    }
}
