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
                new(settingsLocalization.Get("select/action/option/lightAttack"), ((int)FightAction.LightAttack).ToString()),
                new(settingsLocalization.Get("select/action/option/heavyAttack"), ((int)FightAction.HeavyAttack).ToString()),
                new(settingsLocalization.Get("select/action/option/dodge"), ((int)FightAction.Dodge).ToString()),
                new(settingsLocalization.Get("select/action/option/defense"), ((int)FightAction.Defense).ToString()),
                new(settingsLocalization.Get("select/action/option/spell"), ((int)FightAction.Spell).ToString()),
                new(settingsLocalization.Get("select/action/option/skill"), ((int)FightAction.Skill).ToString())
            };
            List<SelectMenuOptionBuilder> rateOptions = new()
            {
                new(settingsLocalization.Get("rate/0"), "0", settingsLocalization.Get("select/rate/desc").Format(0)),
                new(settingsLocalization.Get("rate/1"), "1", settingsLocalization.Get("select/rate/desc").Format(1)),
                new(settingsLocalization.Get("rate/2"), "2", settingsLocalization.Get("select/rate/desc").Format(2)),
                new(settingsLocalization.Get("rate/3"), "3", settingsLocalization.Get("select/rate/desc").Format(3)),
                new(settingsLocalization.Get("rate/4"), "4", settingsLocalization.Get("select/rate/desc").Format(4))
            };
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(settingsLocalization.Get("embed/settings/author"))
                    .WithDescription($"{settingsLocalization.Get("embed/settings/desc/user")}\n\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("embed/use/lightAttack")
                    .Format(settingsLocalization.Get($"rate/{settingsToSave.LightAttack}")), (int)FightAction.LightAttack, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("embed/use/heavyAttack")
                    .Format(settingsLocalization.Get($"rate/{settingsToSave.HeavyAttack}")), (int)FightAction.HeavyAttack, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("embed/use/dodge")
                    .Format(settingsLocalization.Get($"rate/{settingsToSave.Dodge}")), (int)FightAction.Dodge, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("embed/use/defense")
                    .Format(settingsLocalization.Get($"rate/{settingsToSave.Defense}")), (int)FightAction.Defense, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("embed/use/spell")
                    .Format(settingsLocalization.Get($"rate/{settingsToSave.Spell}")), (int)FightAction.Spell, currentAction)}\n" +
                    $"{HighlightIfChoosed(settingsLocalization.Get("embed/use/skill")
                    .Format(settingsLocalization.Get($"rate/{settingsToSave.Skill}")), (int)FightAction.Skill, currentAction)} \n")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("action", actionOptions, settingsLocalization.Get("select/action/name"), row: 0)
                    .WithSelectMenu("rate", rateOptions, settingsLocalization.Get("select/rate/name"), disabled: currentAction is null, row: 1)
                    .WithButton(settingsLocalization.Get("btn/reset"), "reset", ButtonStyle.Secondary, disabled: currentAction is null, row: 2)
                    .WithButton(settingsLocalization.Get("btn/save"), "save", ButtonStyle.Success, disabled: currentAction is null, row: 2)
                    .WithButton(settingsLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger, row: 3)
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
            return current == choosed ? $"{EmojiService.Get("choosePoint")} {str}" : str;
        }
    }
}
