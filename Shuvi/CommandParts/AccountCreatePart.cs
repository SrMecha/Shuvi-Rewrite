using Discord;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class AccountCreatePart
    {
        private static readonly LocalizationLanguagePart _localization = LocalizationService.Get("accountCreatePart");

        public static async Task Start(CustomInteractionContext context)
        {
            var embed = EmbedFactory.CreateUserEmbed(context.User)
                .WithDescription($"{Language.Eng.GetEmoji()}{_localization.Get(Language.Eng).Get("embed/selectLang/desc")}\n" +
                $"{Language.Ru.GetEmoji()}{_localization.Get(Language.Ru).Get("embed/selectLang/desc")}")
                .Build();
            var options = new List<SelectMenuOptionBuilder>();
            foreach (Language currentLang in Enum.GetValues(typeof(Language)))
                options.Add(new SelectMenuOptionBuilder(LocalizationService.Get("names").Get(currentLang).Get($"lang/{currentLang.GetName()}"),
                    ((int)currentLang).ToString(),
                    emote: currentLang.GetEmoji()));
            var components = new ComponentBuilder()
                .WithSelectMenu("select", options, _localization.Get(Language.Eng).Get("select/selectLang/name"))
                .Build();

            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
            var interaction = await context.WaitForButton();
            if (interaction is null)
            {
                await context.CurrentMessage!.RemoveButtonsAsync();
                return;
            }
            var lang = (Language)int.Parse(interaction.Data.Values.First());
            embed = EmbedFactory.CreateUserEmbed(context.User)
                .WithAuthor(_localization.Get(lang).Get("embed/tos/author"))
                .WithDescription(string.Format(_localization.Get(lang).Get("embed/tos/desc"), BotInfoService.TosLink))
                .Build();
            components = new ComponentBuilder()
                .WithButton(_localization.Get(lang).Get("btn/tos/accept"), "accept", ButtonStyle.Success)
                .WithButton(_localization.Get(lang).Get("btn/tos/exit"), "exit", ButtonStyle.Danger)
                .Build();

            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
            await context.LastInteraction.TryDeferAsync();
            interaction = await context.WaitForButton();
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
            await UserDatabase.AddUser(context.User.Id, lang);
            embed = EmbedFactory.CreateUserEmbed(context.User)
                .WithAuthor(_localization.Get(lang).Get("embed/tosAccepted/author"))
                .WithDescription(_localization.Get(lang).Get("embed/tosAccepted/desc"))
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = new ComponentBuilder().Build(); });
        }
    }
}
