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

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IUser user, bool canEdit)
        {
            while (context.LastInteraction is not null)
            {
                var statisticsLocalization = _localizationPart.Get(context.Language);
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(statisticsLocalization.Get("Embed/View/Author").Format(user.Username))
                    .WithDescription($"**{statisticsLocalization.Get("Embed/View/MaxRating")}:** {dbUser.Statistics.MaxRating}\n" +
                    $"**{statisticsLocalization.Get("Embed/View/EnemyKilled")}:** {dbUser.Statistics.TotalEnemyKilled}\n" +
                    $"**{statisticsLocalization.Get("Embed/View/DungeonComplite")}:** {dbUser.Statistics.DungeonComplite}\n" +
                    $"**{statisticsLocalization.Get("Embed/View/Deaths")}:** {dbUser.Statistics.DeathCount}\n\n" +
                    $"{(dbUser.Statistics.DeathCount < 1 ? string.Empty : $"**{statisticsLocalization.Get("Embed/View/LastDeath")}:** " +
                    $"<t:{dbUser.Statistics.LiveTime}:R>\n\n")}" +
                    $"**{statisticsLocalization.Get("Embed/View/AccountCreate")}:** <t:{dbUser.Statistics.CreatedAt}:R>")
                    .Build();
                var components = UserProfilePart.GetProfileSelectMenus(context.Language, dbUser, canEdit);
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
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(statisticsLocalization.Get("Embed/View/Author").Format(pet.Name))
                    .WithDescription($"**{statisticsLocalization.Get("Embed/View/TameTime")}:** <t:{pet.Statistics.TamedAt}:R>")
                    .Build();
                var components = new ComponentBuilder()
                   .WithButton(statisticsLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
