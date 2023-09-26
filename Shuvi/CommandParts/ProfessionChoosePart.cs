using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Factories.Profession;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Enums.User;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class ProfessionChoosePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("professionChoosePart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var chooseLocalization = _localizationPart.Get(context.Language);
            var professions = ProfessionFactory.GetProfessionsByRace(dbUser.Race);
            var options = GetProfessionOptions(professions, context.Language);
            var arrow = 0;
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(chooseLocalization.Get("Embed/Choose/Author"))
                    .WithDescription(chooseLocalization.Get("Embed/Choose/Desc"))
                    .AddField(chooseLocalization.Get("Embed/Choose/Professions"),
                    GetProfessionsString(professions, arrow, context.Language))
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("select", options, chooseLocalization.Get("Select/Choose/Name"), row: 0)
                    .WithButton(chooseLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger, row: 1)
                    .WithButton(chooseLocalization.Get("Btn/Info"), "info", ButtonStyle.Primary, row: 1)
                    .WithButton(chooseLocalization.Get("Btn/Confirm"), "confirm", ButtonStyle.Success, row: 1)
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
                    case "select":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "info":
                        await ProfessionViewPart.Start(context, dbUser, professions[arrow]);
                        break;
                    case "confirm":
                        dbUser.SetProfession(professions[arrow]);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Profession, professions[arrow]));
                        return;
                    case "back":
                        return;
                }
            }
        }

        public static string GetProfessionsString(List<UserProfession> professions, int arrow, Language lang)
        {
            var namesLocalization = LocalizationService.Get("names").Get(lang);
            var result = new List<string>();
            var row = 0;
            foreach (var profession in professions)
            {
                result.Add($"{(row == arrow ? EmojiService.Get("ChoosePoint") : string.Empty)} " +
                    $"{namesLocalization.Get($"Profession/{profession.GetName()}")}");
                row++;
            }
            return string.Join("\n", result);
        }

        public static List<SelectMenuOptionBuilder> GetProfessionOptions(List<UserProfession> professions, Language lang)
        {
            var namesLocalization = LocalizationService.Get("names").Get(lang);
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            foreach (var profession in professions)
            {
                result.Add(new(namesLocalization.Get($"Profession/{profession.GetName()}"), row.ToString()));
                row++;
            }
            return result;
        }
    }
}
