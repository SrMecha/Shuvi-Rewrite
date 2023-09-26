using MongoDB.Bson;
using Shuvi.Classes.Data.Enemy;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Classes.Types.Drop;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Spell;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Spell;

namespace Shuvi.Classes.Types.Enemy
{
    public class DatabaseEnemy : IDatabaseEnemy
    {
        public ObjectId Id { get; private set; }
        public ILocalizedInfo Info { get; private set; }
        public Rank Rank { get; private set; }
        public int RatingGet { get; private set; }
        public int UpgradePoints { get; private set; }
        public IFightActions ActionChances { get; private set; }
        public ISpellInfo Spell { get; private set; }
        public ICharacteristicBonuses Characteristics { get; private set; }
        public IItemsDrop Drop { get; private set; }

        public DatabaseEnemy(EnemyData data)
        {
            Id = data.Id;
            Info = new LocalizedInfo(data.Name, data.Description);
            Rank = data.Rank;
            RatingGet = data.RatingGet;
            UpgradePoints = data.UpgradePoints;
            ActionChances = new FightActions(data.ActionChances);
            Spell = new SpellInfo(data.SpellName);
            Characteristics = new CharacteristicBonuses()
            {
                Strength = data.Strength,
                Agility = data.Agility,
                Luck = data.Luck,
                Intellect = data.Intellect,
                Endurance = data.Endurance,
                Health = data.Health,
                Mana = data.Mana
            };
            Drop = new ItemsDrop(data.Drop);
        }
    }
}
