using Shuvi.Classes.Types.Skill.SkillList;
using Shuvi.Enums.User;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.User;
using System.Collections.ObjectModel;

namespace Shuvi.Classes.Factories.Skill
{
    public static class SkillFactory
    {
        private static IReadOnlyDictionary<string, ISkill>? _skills;
        private static IReadOnlyDictionary<UserProfession, List<ISkill>>? _skillsByProfession;

        public static void SetDictionary(IReadOnlyDictionary<string, ISkill> skills)
        {
            _skills = skills;
            ConfigureSkillsByProfession(skills);
        }

        public static ISkill GetSkill(string skillName)
        {
            if (_skills!.TryGetValue(skillName, out var result))
                return result.CreateCopy();
            return new VoidSkill();
        }

        private static void ConfigureSkillsByProfession(IReadOnlyDictionary<string, ISkill> skills)
        {
            var result = new Dictionary<UserProfession, List<ISkill>>();
            foreach (var (_, skill) in skills)
            {
                if (result.ContainsKey(skill.Profession))
                    result[skill.Profession].Add(skill);
                else
                    result.Add(skill.Profession, new() { skill });
            }
            _skillsByProfession = new ReadOnlyDictionary<UserProfession, List<ISkill>>(result);
        }

        public static List<ISkill> GetAvailableSkills(IDatabaseUser user)
        {
            var result = new List<ISkill>();
            foreach (var skill in _skillsByProfession!.GetValueOrDefault(user.Profession, new()))
            {
                if (skill.Requirements.IsMeetRequirements(user))
                    result.Add(skill);
            }
            return result;
        }
    }
}
