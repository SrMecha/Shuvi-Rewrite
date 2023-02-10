using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Skill
{
    public interface ISkill
    {
        public interface ISkill
        {
            public ILocalizedInfo Info { get; }
            public bool CanUse { get; }

            public string GetStatus(ICombatEntity owner);
            protected IActionResult OnSkillUse(ICombatEntity owner, ICombatEntity target);
            public IActionResult UseSkill(ICombatEntity owner, ICombatEntity target);
            public void Update(ICombatEntity owner);
        }
    }
}
