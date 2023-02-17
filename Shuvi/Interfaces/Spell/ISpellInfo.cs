using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Spell
{
    public interface ISpellInfo
    {
        public ILocalizedInfo Info { get; }

        public ISpell GetSpell();
    }
}
