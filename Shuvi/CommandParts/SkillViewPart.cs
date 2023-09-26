using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class SkillViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("skillViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, ISkill skill)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(viewLocalization.Get("Embed/View/Author"))
                    .WithDescription($"{viewLocalization.Get("Embed/View/SkillName").Format(skill.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("Embed/View/Desc").Format(skill.Info.GetDescription(context.Language))}")
                    .AddField(viewLocalization.Get("Embed/View/Requirements"),
                    skill.Requirements.GetRequirementsInfo(context.Language, dbUser).Description)
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger)
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
    }
}
