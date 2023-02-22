using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.Combat
{
    public class CombatPlayer : CombatEntity, ICombatPlayer
    {
        public ISkill Skill { get; private set; }
        public IDropInventory Inventory { get; private set; }

        public CombatPlayer(IDatabaseUser user, string name) 
        {
            Name = name;
            Rank = user.Rating.Rank;
            Characteristics = new EntityCharacteristics<INotRestorableCharacteristic>(user.Characteristic,
                new NotRestorableCharacteristic(user.Characteristic.Health.GetCurrent(), user.Characteristic.Health.GetCurrent()),
                new NotRestorableCharacteristic(user.Characteristic.Mana.GetCurrent(), user.Characteristic.Mana.GetCurrent()));
            Spell = user.Spell.GetSpell();
            Skill = user.Skill.GetSkill();
            Actions = user.ActionChances;
            Inventory = new DropInventory();
        }
        public IActionResult UseSkill(ICombatEntity target, Language lang)
        {
            return Skill.UseSkill(this, target, lang);
        }
        public override IResultStorage Update(Language lang)
        {
            var result =  base.Update(lang);
            result.Add(Skill.Update(this, lang));
            return result;
        }
    }
}
