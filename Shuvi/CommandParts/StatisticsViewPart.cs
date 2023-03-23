using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class StatisticsViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("statisticsViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            while (context.LastInteraction is not null)
            {
                var statisticsLocalization = _localizationPart.Get(context.Language);
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(statisticsLocalization.Get("embed/view/author").Format(context.User.Username))
                    .WithDescription($"**{statisticsLocalization.Get("embed/view/maxRating")}:** {dbUser.Statistics.MaxRating}\n" +
                    $"**{statisticsLocalization.Get("embed/view/enemyKilled")}:** {dbUser.Statistics.TotalEnemyKilled}\n" +
                    $"**{statisticsLocalization.Get("embed/view/dungeonComplite")}:** {dbUser.Statistics.DungeonComplite}\n" +
                    $"**{statisticsLocalization.Get("embed/view/deaths")}:** {dbUser.Statistics.DeathCount}\n\n" +
                    $"{(dbUser.Statistics.DeathCount < 1 ? string.Empty : $"**{statisticsLocalization.Get("embed/view/lastDeath")}:** " +
                    $"<t:{dbUser.Statistics.LiveTime}:R>\n\n")}" +
                    $"**{statisticsLocalization.Get("embed/view/accountCreate")}:** <t:{dbUser.Statistics.CreatedAt}:R>")
                    .Build();
                var components = new ComponentBuilder()
                   .WithButton(statisticsLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
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
        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IDatabasePet pet)
        {
            while (context.LastInteraction is not null)
            {
                var statisticsLocalization = _localizationPart.Get(context.Language);
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(statisticsLocalization.Get("embed/view/author").Format(pet.Name))
                    .WithDescription($"**{statisticsLocalization.Get("embed/view/tameTime")}:** <t:{pet.Statistics.TamedAt}:R>")
                    .Build();
                var components = new ComponentBuilder()
                   .WithButton(statisticsLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
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
