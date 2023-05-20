using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Classes.Types.Magic;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class SpellViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("spellViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, ISpell spell)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(viewLocalization.Get("embed/view/author"))
                    .WithDescription($"{viewLocalization.Get("embed/view/spellName").Format(spell.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("embed/view/magicType").Format(new MagicInfo(spell.MagicType).Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("embed/view/desc").Format(spell.Info.GetDescription(context.Language))}")
                    .AddField(viewLocalization.Get("embed/view/requirements"),
                    spell.Requirements.GetRequirementsInfo(context.Language, dbUser))
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("btn/back"), "back", ButtonStyle.Danger)
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
                switch (interaction.Data.CustomId)
                {
                    case "back":
                        return;
                }
            }
        }
    }
}
