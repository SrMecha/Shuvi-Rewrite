using Shuvi.Enums.Magic;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Magic
{
    public interface IMagicInfo
    {
        public MagicType MagicType { get; }
        public ILocalizedInfo Info { get; }

        public List<ISpell> GetAvailableSpells(IDatabaseUser user);
        public List<ISpell> GetAllSpells();
    }
}
