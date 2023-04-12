using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;
using Shuvi.Services.StaticServices.Map;

namespace Shuvi.CommandParts
{
    public static class LocationViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("locationViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IUser user)
        {
            var locationLocalization = _localizationPart.Get(context.Language);
            WorldMap.Regions.ElementAt(0);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(locationLocalization.Get("embed/view/author").Format(user.Username))
                    .WithDescription($"**{locationLocalization.Get("embed/view/region")}:** " +
                    $"{dbUser.Location.GetRegion().Info.GetName(context.Language)}\n" +
                    $"**{locationLocalization.Get("embed/view/location")}:** " +
                    $"{dbUser.Location.GetLocation().Info.GetName(context.Language)}")
                    .WithImageUrl(dbUser.Location.GetLocation().PictureURL)
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(locationLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
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
                    case "exit":
                        return;
                    default:
                        return;
                }
            }
        }
    }
}
