using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Factories.Skill;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Enums.User;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class ProfessionViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("professionViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, UserProfession profession)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var skills = SkillFactory.GetAvailableSkills(dbUser, profession);
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(namesLocalization.Get($"Profession/{profession.GetName()}"))
                    .WithDescription($"**{viewLocalization.Get("Embed/View/Desc")}**\n{namesLocalization.Get($"Profession/{profession.GetName()}/Desc")}")
                    .AddField(viewLocalization.Get("Embed/View/Skills"),
                    $"{GetSkillsString(skills, context.Language)}\n\n__{viewLocalization.Get("Embed/View/SkillsDesc")}__")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger, row: 1)
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

        public static string GetSkillsString(List<ISkill> skills, Language lang)
        {
            var result = new List<string>();
            var row = 0;
            foreach (var skill in skills)
            {
                result.Add(skill.Info.GetName(lang));
                row++;
            }
            return result.Count > 0 ? string.Join("\n", result) : _localizationPart.Get(lang).Get("Embed/Select/Empty");
        }
    }
}
