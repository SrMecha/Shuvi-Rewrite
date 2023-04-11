using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Skill
{
    public interface ISkillInfo
    {
        public ILocalizedInfo Info { get; }
        public string? SkillId { get; }

        public ISkill GetSkill();
        public bool HaveSkill();
    }
}
