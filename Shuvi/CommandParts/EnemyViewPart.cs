using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class EnemyViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("enemyViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IDatabaseEnemy enemy)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var part = "empty";
            var components = GetButtons(dbUser, enemy, context.Language);
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(enemy.Info.GetName(context.Language))
                    .WithDescription($"{enemy.Info.GetDescription(context.Language)}\n" +
                    $"{viewLocalization.Get("embed/main/rank").Format(enemy.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("embed/main/ratingGet").Format(enemy.RatingGet)}\n" +
                    $"{viewLocalization.Get("embed/main/killsCount").Format(dbUser.Statistics.GetEnemyKills(enemy.Id))}")
                    .AddField(GetPartInfo(dbUser, enemy, part, context.Language))
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
                        part = interaction.Data.Values.First();
                        break;
                    default:
                        return;
                }
            }
        }

        public static EmbedFieldBuilder GetPartInfo(IDatabaseUser dbUser, IDatabaseEnemy enemy, string part, Language lang)
        {
            var viewLocalization = _localizationPart.Get(lang);
            var namesLocalization = LocalizationService.Get("names").Get(lang);
            return part switch
            {
                "characteristics" => new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("embed/characteristics/name"))
                    .WithValue($"**{namesLocalization.Get("strength")}:** {enemy.Characteristics.Strength}\n" +
                    $"{namesLocalization.Get("agility")}: {enemy.Characteristics.Agility}\n" +
                    $"{namesLocalization.Get("luck")}: {enemy.Characteristics.Luck}\n" +
                    $"{namesLocalization.Get("intellect")}: {enemy.Characteristics.Intellect}\n" +
                    $"{namesLocalization.Get("endurance")}: {enemy.Characteristics.Endurance}\n" +
                    $"{namesLocalization.Get("health")}: {enemy.Characteristics.Health}\n" +
                    $"{namesLocalization.Get("mana")}: {enemy.Characteristics.Mana}"),
                "abilities" => new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("embed/abilities/name"))
                    .WithValue($"{viewLocalization.Get("embed/abilities/spell/name").Format(enemy.Spell.Info.GetName(lang))}\n" +
                    $"{(enemy.Spell.HaveSpell() ?
                    $"{viewLocalization.Get("embed/abilities/spell/magicType").Format(enemy.Spell.MagicInfo.Info.GetName(lang))}\n" +
                    $"{viewLocalization.Get("embed/abilities/spell/desc").Format(enemy.Spell.Info.GetDescription(lang))}"
                    : string.Empty)}"),
                "drop" => new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("embed/drop/name"))
                    .WithValue(enemy.Drop.GetChancesInfo(dbUser.Characteristics.Luck, lang)),
                "actions" => new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("embed/action/name"))
                    .WithValue($"{viewLocalization.Get("embed/action/lightAttack").Format(viewLocalization.Get($"rate/{enemy.ActionChances.LightAttack}"))}\n" +
                    $"{viewLocalization.Get("embed/action/heavyAttack").Format(viewLocalization.Get($"rate/{enemy.ActionChances.HeavyAttack}"))}\n" +
                    $"{viewLocalization.Get("embed/action/dodge").Format(viewLocalization.Get($"rate/{enemy.ActionChances.Dodge}"))}\n" +
                    $"{viewLocalization.Get("embed/action/defense").Format(viewLocalization.Get($"rate/{enemy.ActionChances.Defense}"))}\n" +
                    $"{viewLocalization.Get("embed/action/spell").Format(viewLocalization.Get($"rate/{enemy.ActionChances.Spell}"))}\n"),
                _ => new EmbedFieldBuilder()
                    .WithName("** **")
                    .WithValue("** **")
            };
        }

        public static MessageComponent GetButtons(IDatabaseUser dbUser, IDatabaseEnemy enemy, Language lang)
        {
            var viewLocalization = _localizationPart.Get(lang);
            var result = new ComponentBuilder();
            var options = new List<SelectMenuOptionBuilder>();
            if (EnemyInfoService.CanViewEnemyCharacteristics(dbUser, enemy))
                options.Add(new(viewLocalization.Get("select/info/option/characteristics"), "characteristics"));
            if (EnemyInfoService.CanViewEnemyAbilities(dbUser, enemy))
                options.Add(new(viewLocalization.Get("select/info/option/abilites"), "abilities"));
            if (EnemyInfoService.CanViewEnemyDrop(dbUser, enemy))
                options.Add(new(viewLocalization.Get("select/info/option/drop"), "drop"));
            if (EnemyInfoService.CanViewEnemyActionChances(dbUser, enemy))
                options.Add(new(viewLocalization.Get("select/info/option/actions"), "actions"));
            if (options.Count > 0)
                return result
                    .WithSelectMenu("select", options, viewLocalization.Get("select/info/name"), row: 0)
                    .WithButton(viewLocalization.Get("btn/back"), "back", ButtonStyle.Danger, row: 1)
                    .Build();
            options.Add(new("EMPTY", "empty"));
            return result
                    .WithSelectMenu("select", options, viewLocalization.Get("select/info/noFound"), disabled: true, row: 0)
                    .WithButton(viewLocalization.Get("btn/back"), "back", ButtonStyle.Danger, row: 1)
                    .Build();

        }
    }
}
