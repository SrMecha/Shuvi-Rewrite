﻿using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.Spell
{
    public interface ISpell
    {
        public ILocalizedInfo Info { get; }
        public IBaseRequirements Requirements { get; }
        public int Cost { get; }

        public bool CanCast(ICombatEntity player);
        protected IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang);
        public IActionResult Cast(ICombatEntity player, ICombatEntity target, Language lang);
        public IActionResult? Update(Language lang);
        public ISpell CreateCopy();
    }
}
