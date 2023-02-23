namespace Shuvi.Interfaces.Spell
{
    public interface IChangableSpellInfo : ISpellInfo
    {
        public void SetSpell(string? spellName);

    }
}
