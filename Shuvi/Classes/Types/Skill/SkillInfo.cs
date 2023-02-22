﻿using Shuvi.Classes.Factories.Skill;
using Shuvi.Classes.Types.Localization;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Skill;

namespace Shuvi.Classes.Types.Skill
{
    public class SkillInfo : ISkillInfo
    {
        protected string? _skillName;

        public ILocalizedInfo Info { get; }

        public SkillInfo(string? skillName)
        {
            _skillName = skillName;
            Info = new CachedLocalizedInfo("skills", skillName);
        }
        public ISkill GetSkill()
        {
            return SkillFactory.GetSkill(_skillName ?? string.Empty);
        }
    }
}
