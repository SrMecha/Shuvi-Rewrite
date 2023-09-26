using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Factories.Skill;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class SkillChangePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("skillChangePart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var changeLocalization = _localizationPart.Get(context.Language);
            var skills = SkillFactory.GetAvailableSkills(dbUser);
            var skillOptions = GetSkillOptions(skills, context.Language);
            var haveSkills = skillOptions.Count > 0;
            if (!haveSkills)
                skillOptions.Add(new("EMPTY", "EMPTY"));
            var arrow = 0;
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(changeLocalization.Get("Embed/Select/Author"))
                    .WithDescription($"{changeLocalization.Get("Embed/Select/CurrentSkill").Format(dbUser.Skill.Info.GetName(context.Language))}\n\n" +
                        $"{(haveSkills ? $"{changeLocalization.Get("Embed/Select/AvailableSkills")}\n" +
                        $"{GetSkillsString(skills, arrow, context.Language)}" : changeLocalization.Get("Embed/Select/Empty"))}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("choose", skillOptions, changeLocalization.Get("Select/Skill/Name"), disabled: !haveSkills, row: 0)
                    .WithButton(changeLocalization.Get("Btn/Equip"), "equip", ButtonStyle.Success, disabled: !haveSkills, row: 1)
                    .WithButton(changeLocalization.Get("Btn/Info"), "info", ButtonStyle.Primary, disabled: !haveSkills, row: 1)
                    .WithButton(changeLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger, row: 2)
                    .WithButton(changeLocalization.Get("Btn/Unequip"), "unequip", ButtonStyle.Secondary, disabled: !dbUser.Skill.HaveSkill(), row: 2)
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
                    case "choose":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "equip":
                        var skillName = skills[arrow].SkillName;
                        dbUser.Skill.SetSkill(skillName);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Skill, skillName));
                        break;
                    case "unequip":
                        dbUser.Skill.SetSkill(null);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Skill, null));
                        break;
                    case "info":
                        await SkillViewPart.Start(context, dbUser, skills[arrow]);
                        break;
                    case "back":
                        return;
                }
            }
        }

        public static string GetSkillsString(List<ISkill> skills, int arrow, Language lang)
        {
            var result = new List<string>();
            var row = 0;
            foreach (var skill in skills)
            {
                result.Add($"{(row == arrow ? EmojiService.Get("ChoosePoint") : string.Empty)} {skill.Info.GetName(lang)}");
                row++;
            }
            return result.Count > 0 ? string.Join("\n", result) : _localizationPart.Get(lang).Get("Embed/Select/Empty");
        }

        public static List<SelectMenuOptionBuilder> GetSkillOptions(List<ISkill> skills, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            foreach (var skill in skills)
            {
                var description = skill.Info.GetDescription(lang);
                if (description.Length > 70)
                    description = $"{description[..70]}...";
                result.Add(new(skill.Info.GetName(lang), row.ToString(), description));
                row++;
            }
            return result;
        }
    }
}
