using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.Status;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Spell.SpellList
{
    public class SpellBase : ISpell
    {

        public ILocalizedInfo Info { get; }
        public IBaseRequirements Requirements { get; }
        public int Cost { get; }

        public SpellBase()
        {
            Info = new CachedLocalizedInfo("spell");
            Requirements = new Req
        }
        public virtual bool CanCast(ICombatEntity player)
        {
            return Cost >= player.Characteristics.Mana.Now;
        }
        public virtual IActionResult OnCast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            return new ActionResult(string.Format(LocalizationService.Get("status").Get(lang).Get("spell/voidCast"), player.Name));
        }
        public IActionResult Cast(ICombatEntity player, ICombatEntity target, Language lang)
        {
            player.Characteristics.Mana.Reduce(Cost);
            return OnCast(player, target, lang);
        }
    }
}
