﻿using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Magic;

namespace Shuvi.Interfaces.Spell
{
    public interface ISpellInfo
    {
        public ILocalizedInfo Info { get; }
        public IMagicInfo MagicInfo { get; }
        public string? SpellId { get; }

        public ISpell GetSpell();
        public bool HaveSpell();
    }
}
