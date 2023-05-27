using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class BotInfoPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("botInfoPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var infoLocalization = _localizationPart.Get(context.Language);
            var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser, false, false)
                .WithDescription($"{infoLocalization.Get("embed/info/players").Format(BotInfoService.PlayerCount)}\n" +
                $"{infoLocalization.Get("embed/info/guilds").Format(context.Client.Guilds.Count)}\n\n" +
                $"{infoLocalization.Get("embed/info/links")}")
                .WithFooter(infoLocalization.Get("embed/info/version").Format(BotInfoService.Version))
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; });
            await context.LastInteraction.TryDeferAsync();
        }
    }
}
