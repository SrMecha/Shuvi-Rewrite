using MongoDB.Bson;
using Shuvi.Classes.Data.Enemy;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Drop;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Spell;
using Shuvi.Enums.Actions;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Actions;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Spell;
using ZstdSharp.Unsafe;

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
        public IEntityCharacteristics<IDynamicCharacteristic> Characteristics { get; private set; }
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
            Characteristics = new EntityCharacteristics<IDynamicCharacteristic>(
                data.Strength, data.Agility, data.Luck, data.Intellect, data.Endurance,
                new DynamicCharacteristic(data.Health), new DynamicCharacteristic(data.Mana));
            Drop = new ItemsDrop(data.Drop);
        }
    }
}
