namespace Shuvi.Interfaces.Skill
{
    public interface IChangableSkillInfo : ISkillInfo
    {
        public void SetSkill(string? skillName);
    }
}
