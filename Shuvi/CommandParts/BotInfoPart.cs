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
            var embed = EmbedFactory.CreateBotOwnerEmbed()
                .WithDescription($"{infoLocalization.Get("Embed/Info/Players").Format(BotInfoService.PlayerCount)}\n" +
                $"{infoLocalization.Get("Embed/Info/Guilds").Format(context.Client.Guilds.Count)}\n\n" +
                $"{infoLocalization.Get("Embed/Info/Links")}")
                .WithFooter(infoLocalization.Get("Embed/Info/Version").Format(BotInfoService.Version))
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; });
            await context.LastInteraction.TryDeferAsync();
        }
    }
}
