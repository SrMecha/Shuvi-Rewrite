using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class AccountCreatePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("accountCreatePart");

        public static async Task Start(CustomInteractionContext context)
        {
            var createLocalization = _localizationPart.Get(context.Language);
            var embed = EmbedFactory.CreateUserEmbed(context.User)
                .WithAuthor(createLocalization.Get("embed/tos/author"))
                .WithDescription(string.Format(createLocalization.Get("embed/tos/desc"), BotInfoService.TosLink))
                .Build();
            var components = new ComponentBuilder()
                .WithButton(createLocalization.Get("btn/tos/accept"), "accept", ButtonStyle.Success)
                .WithButton(createLocalization.Get("btn/tos/exit"), "exit", ButtonStyle.Danger)
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
                case "accept":
                    break;
                case "exit":
                    await context.Interaction.DeleteOriginalResponseAsync();
                    return;
                default:
                    return;
            }
            await UserDatabase.AddUser(context.User.Id);
            embed = EmbedFactory.CreateUserEmbed(context.User)
                .WithAuthor(createLocalization.Get("embed/tosAccepted/author"))
                .WithDescription(createLocalization.Get("embed/tosAccepted/desc"))
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = new ComponentBuilder().Build(); });
        }
    }
}
