﻿using MongoDB.Bson;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Interfaces.Enemy
{
    public interface IDatabaseEnemy
    {
        public ObjectId Id { get; }
        public ILocalizedInfo Info { get; }
        public Rank Rank { get; }
        public int RatingGet { get; }
        public int UpgradePoints { get; }
        public IFightActions ActionChances { get; }
        public ISpellInfo Spell { get; }
        public ICharacteristicBonuses Characteristics { get; }
        public IItemsDrop Drop { get; }
    }
}
