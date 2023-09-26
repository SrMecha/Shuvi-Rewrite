using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Actions;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Actions;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class FightSettingsPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("fightSettingsPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var settingsLocalization = _localizationPart.Get(context.Language);
            int? currentAction = null;
            var settingsToSave = new UserFightActions(dbUser.ActionChances);
            List<SelectMenuOptionBuilder> actionOptions = new()
            {
                new(settingsLocalization.Get("Select/Action/Option/LightAttack"), ((int)FightAction.LightAttack).ToString()),
                new(settingsLocalization.Get("Select/Action/Option/HeavyAttack"), ((int)FightAction.HeavyAttack).ToString()),
                new(settingsLocalization.Get("Select/Action/Option/Dodge"), ((int)FightAction.Dodge).ToString()),
                new(settingsLocalization.Get("Select/Action/Option/Defense"), ((int)FightAction.Defense).ToString()),
                new(settingsLocalization.Get("Select/Action/Option/Spell"), ((int)FightAction.Spell).ToString()),
                new(settingsLocalization.Get("Select/Action/Option/Skill"), ((int)FightAction.Skill).ToString())
            };
            List<SelectMenuOptionBuilder> rateOptions = new()
            {
                new(settingsLocalization.Get("Rate/0"), "0", settingsLocalization.Get("Select/Rate/Desc").Format(0)),
                new(settingsLocalization.Get("Rate/1"), "1", settingsLocalization.Get("Select/Rate/Desc").Format(1)),
                new(settingsLocalization.Get("Rate/2"), "2", settingsLocalization.Get("Select/Rate/Desc").Format(2)),
                new(settingsLocalization.Get("Rate/3"), "3", settingsLocalization.Get("Select/Rate/Desc").Format(3)),
                new(settingsLocalization.Get("Rate/4"), "4", settingsLocalization.Get("Select/Rate/Desc").Format(4))
            };
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(settingsLocalization.Get("Embed/Settings/Author"))
                    .WithDescription($"{settingsLocalization.Get("Embed/Settings/Desc/User")}\n\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("Embed/Use/LightAttack")
                    .Format(settingsLocalization.Get($"Rate/{settingsToSave.LightAttack}")), (int)FightAction.LightAttack, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("Embed/Use/HeavyAttack")
                    .Format(settingsLocalization.Get($"Rate/{settingsToSave.HeavyAttack}")), (int)FightAction.HeavyAttack, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("Embed/Use/Dodge")
                    .Format(settingsLocalization.Get($"Rate/{settingsToSave.Dodge}")), (int)FightAction.Dodge, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("Embed/Use/Defense")
                    .Format(settingsLocalization.Get($"Rate/{settingsToSave.Defense}")), (int)FightAction.Defense, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("Embed/Use/Spell")
                    .Format(settingsLocalization.Get($"Rate/{settingsToSave.Spell}")), (int)FightAction.Spell, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("Rmbed/Use/Skill")
                    .Format(settingsLocalization.Get($"Rate/{settingsToSave.Skill}")), (int)FightAction.Skill, currentAction)} \n")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("action", actionOptions, settingsLocalization.Get("Select/Action/Name"), row: 0)
                    .WithSelectMenu("rate", rateOptions, settingsLocalization.Get("Select/Rate/Name"), disabled: currentAction is null, row: 1)
                    .WithButton(settingsLocalization.Get("Btn/Reset"), "reset", ButtonStyle.Secondary, disabled: currentAction is null, row: 2)
                    .WithButton(settingsLocalization.Get("Btn/Save"), "save", ButtonStyle.Success, disabled: currentAction is null, row: 2)
                    .WithButton(settingsLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger, row: 3)
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
                    case "action":
                        currentAction = int.Parse(interaction.Data.Values.First());
                        break;
                    case "rate":
                        settingsToSave.SetAction((FightAction)currentAction!, int.Parse(interaction.Data.Values.First()));
                        break;
                    case "reset":
                        settingsToSave = new UserFightActions(dbUser.ActionChances);
                        currentAction = null;
                        break;
                    case "save":
                        dbUser.ActionChances.LightAttack = settingsToSave.LightAttack;
                        dbUser.ActionChances.HeavyAttack = settingsToSave.HeavyAttack;
                        dbUser.ActionChances.Dodge = settingsToSave.Dodge;
                        dbUser.ActionChances.Defense = settingsToSave.Defense;
                        dbUser.ActionChances.Spell = settingsToSave.Spell;
                        dbUser.ActionChances.Skill = settingsToSave.Skill;
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.ActionChances, new()
                            {
                                LightAttack = settingsToSave.LightAttack,
                                HeavyAttack = settingsToSave.HeavyAttack,
                                Dodge = settingsToSave.Dodge,
                                Defense = settingsToSave.Defense,
                                Spell = settingsToSave.Spell,
                                Skill = settingsToSave.Skill
                            }));
                        return;
                    case "exit":
                        return;
                    default:
                        return;
                }
            }
        }
        private static string HighlightIfChoosed(string str, int current, int? choosed)
        {
            return current == choosed ? $"{EmojiService.Get("ChoosePoint")} {str}" : str;
        }
    }
}
