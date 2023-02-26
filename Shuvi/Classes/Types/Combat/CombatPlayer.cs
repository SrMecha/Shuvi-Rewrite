using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Enums.Actions;
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
            Characteristics = new EntityCharacteristics<INotRestorableCharacteristic>(user.Characteristics,
                new NotRestorableCharacteristic(user.Characteristics.Health.GetCurrent(), user.Characteristics.Health.GetCurrent()),
                new NotRestorableCharacteristic(user.Characteristics.Mana.GetCurrent(), user.Characteristics.Mana.GetCurrent()));
            Spell = user.Spell.GetSpell();
            Skill = user.Skill.GetSkill();
            Actions = user.ActionChances;
            Inventory = new DropInventory();
        }
        public override IActionResult RandomAction(ICombatEntity target, Language lang)
        {
            return Actions.GetRandomAction() switch
            {
                FightAction.LightAttack => DealLightDamage(target, lang),
                FightAction.HeavyAttack => DealHeavyDamage(target, lang),
                FightAction.Dodge => PreparingForDodge(target, lang),
                FightAction.Defense => PreparingForDefense(target, lang),
                FightAction.Spell => Spell.CanCast(this) ? CastSpell(target, lang) : DealLightDamage(target, lang),
                FightAction.Skill => Skill.CanUse(this) ? UseSkill(target, lang) : DealLightDamage(target, lang),
                _ => DealLightDamage(target, lang),
            };
        }
        public IActionResult UseSkill(ICombatEntity target, Language lang)
        {
            return Skill.UseSkill(this, target, lang);
        }
        public override IResultStorage Update(Language lang)
        {
            var result = base.Update(lang);
            result.Add(Skill.Update(this, lang));
            return result;
        }
    }
}
