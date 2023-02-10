using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Spell
{
    public interface IUserSpellInfo
    {
        public ILocalizedInfo Info { get; }
        public ISpell GetSpell { get; }

    }
}
