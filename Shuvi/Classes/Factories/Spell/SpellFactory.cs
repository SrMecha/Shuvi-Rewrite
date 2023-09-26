using Shuvi.Classes.Types.Spell.SpellList;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Magic;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.User;
using System.Collections.ObjectModel;

namespace Shuvi.Classes.Factories.Spell
{
    public static class SpellFactory
    {
        private static IReadOnlyDictionary<string, ISpell>? _spells;
        private static IReadOnlyDictionary<MagicType, List<ISpell>>? _spellsByMagic;

        public static void SetDictionary(IReadOnlyDictionary<string, ISpell> spells)
        {
            _spells = spells;
            ConfigureSpellsByType(spells);
        }
        private static void ConfigureSpellsByType(IReadOnlyDictionary<string, ISpell> spells)
        {
            var result = new Dictionary<MagicType, List<ISpell>>();
            foreach (var (_, spell) in spells)
            {
                if (result.ContainsKey(spell.MagicType))
                    result[spell.MagicType].Add(spell);
                else
                    result.Add(spell.MagicType, new() { spell });
            }
            _spellsByMagic = new ReadOnlyDictionary<MagicType, List<ISpell>>(result);
        }
        public static ISpell GetSpell(string spellName)
        {
            if (_spells!.TryGetValue(spellName, out var result))
                return result.CreateCopy();
            return new VoidSpell();
        }
        public static List<ISpell> GetAvailableSpells(IDatabaseUser user)
        {
            var result = new List<ISpell>();
            foreach (var spell in _spellsByMagic!.GetValueOrDefault(user.MagicInfo.MagicType, new()))
            {
                var requirements = spell.Requirements.GetRequirementsInfo(Language.Eng, user);
                if (requirements.IsMeetRequirements)
                    result.Add(spell);
            }
            return result;
        }
        public static List<ISpell> GetSpells(MagicType magicType)
        {
            return _spellsByMagic!.GetValueOrDefault(magicType, new());
        }
    }
}
