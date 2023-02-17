using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Spell
{
    public interface IUserSpellInfo : ISpellInfo
    {
        public void SetSpell(string? spellName);

    }
}
