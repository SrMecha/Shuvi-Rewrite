using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Magic;
using Shuvi.Enums.Magic;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Magic;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Types.Spell
{
    public class SpellInfo : ISpellInfo
    {
        protected string? _spellName;

        public ILocalizedInfo Info { get; protected set; }
        public IMagicInfo MagicInfo { get; protected set; }

        public SpellInfo(string? spellName, MagicType magicType = MagicType.None)
        {
            _spellName = spellName;
            Info = new CachedLocalizedInfo("spells", spellName);
            MagicInfo = new MagicInfo(magicType);
        }
        public ISpell GetSpell()
        {
            return SpellFactory.GetSpell(_spellName ?? string.Empty);
        }
        public bool HaveSpell()
        {
            return _spellName is not null;
        }
    }
}
