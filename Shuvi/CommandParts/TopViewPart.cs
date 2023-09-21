using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Top;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;
using Shuvi.Services.StaticServices.Top;

namespace Shuvi.CommandParts
{
    public static class TopViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("topViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, UserTopType topType)
        {
            var topLocalization = _localizationPart.Get(context.Language);
            var currentPage = 0;
            var maxPage = TopService.GetTotalPages(topType);
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(topLocalization.Get("Embed/View/Author").Format(topLocalization.Get($"Top/{topType.GetName()}")))
                    .WithDescription($"{GetTopString(topType, currentPage, context.Language)}\n\n" +
                    $"{topLocalization.Get("Embed/View/TopUpdateTime").Format(TopService.GetUpdateTime(topType))}")
                    .WithFooter($"{context.User.Username}#{context.User.Discriminator} | " +
                    $"{topLocalization.Get("Embed/View/Page").Format(currentPage + 1, maxPage)}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: currentPage < 1, row: 0)
                    .WithButton(topLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger, row: 0)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: currentPage >= maxPage - 1, row: 0)
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
                    case "<":
                        currentPage--;
                        break;
                    case ">":
                        currentPage++;
                        break;
                    case "exit":
                        await context.Interaction.DeleteOriginalResponseAsync();
                        return;
                }
            }
        }

        public static string GetTopString(UserTopType topType, int page, Language lang)
        {
            var result = new List<string>();
            foreach (var member in TopService.GetTopMembersInPage(topType, page))
                result.Add($"**#{member.Place}** {member.Name} | {member.Amount}");
            return result.Count < 1 ? _localizationPart.Get(lang).Get("Embed/View/Empty") : string.Join("\n", result);
        }
    }
}
