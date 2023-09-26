using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Drop;
using Shuvi.Interfaces.Enemy;

namespace Shuvi.Classes.Types.Combat
{
    public class CombatEnemy : CombatEntity, ICombatEnemy
    {
        public int RatingGet { get; }
        public IItemsDrop Drop { get; }

        public CombatEnemy(IDatabaseEnemy enemy, Language lang)
        {
            Name = enemy.Info.GetName(lang);
            Rank = enemy.Rank;
            Characteristics = new EntityCharacteristics<INotRestorableCharacteristic>() 
            {
                Strength = enemy.Characteristics.Strength,
                Agility = enemy.Characteristics.Agility,
                Luck = enemy.Characteristics.Luck,
                Intellect = enemy.Characteristics.Intellect,
                Endurance = enemy.Characteristics.Endurance,
                Health = new NotRestorableCharacteristic(enemy.Characteristics.Health, enemy.Characteristics.Health),
                Mana = new NotRestorableCharacteristic(enemy.Characteristics.Mana, enemy.Characteristics.Mana)
            };
            Spell = enemy.Spell.GetSpell();
            Actions = enemy.ActionChances;
            RatingGet = enemy.RatingGet;
            Drop = enemy.Drop;
            Update(Language.Eng);
        }
    }
}
