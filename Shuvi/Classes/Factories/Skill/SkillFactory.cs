using Shuvi.Classes.Types.Skill.SkillList;
using Shuvi.Interfaces.Skill;

namespace Shuvi.Classes.Factories.Skill
{
    public static class SkillFactory
    {
        private static IReadOnlyDictionary<string, ISkill>? _skills;

        public static void SetDictionary(IReadOnlyDictionary<string, ISkill> skills)
        {
            _skills = skills;
        }

        public static ISkill GetSpell(string skillName)
        {
            if (_skills!.TryGetValue(skillName, out var result))
                return result.CreateCopy();
            return new VoidSkill();
        }
    }
}
