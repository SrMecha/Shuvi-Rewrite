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
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(enemy.Info.GetName(context.Language))
                    .WithDescription($"{enemy.Info.GetDescription(context.Language)}\n" +
                    $"{viewLocalization.Get("Embed/Main/Rank").Format(enemy.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("Embed/Main/RatingGet").Format(enemy.RatingGet)}\n" +
                    $"{viewLocalization.Get("Embed/Main/KillsCount").Format(dbUser.Statistics.GetEnemyKills(enemy.Id))}")
                    .WithFields(GetPartFields(dbUser, enemy, part, context.Language))
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

        public static List<EmbedFieldBuilder> GetPartFields(IDatabaseUser dbUser, IDatabaseEnemy enemy, string part, Language lang)
        {
            var viewLocalization = _localizationPart.Get(lang);
            var namesLocalization = LocalizationService.Get("names").Get(lang);
            return part switch
            {
                "characteristics" => new() 
                {
                    new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("Embed/Characteristics/Name"))
                    .WithValue($"**{namesLocalization.Get("Strength")}:** {enemy.Characteristics.Strength}\n" +
                    $"**{namesLocalization.Get("Agility")}:** {enemy.Characteristics.Agility}\n" +
                    $"**{namesLocalization.Get("Luck")}:** {enemy.Characteristics.Luck}\n" +
                    $"**{namesLocalization.Get("Intellect")}:** {enemy.Characteristics.Intellect}\n" +
                    $"**{namesLocalization.Get("Endurance")}:** {enemy.Characteristics.Endurance}\n" +
                    $"**{namesLocalization.Get("Health")}:** {enemy.Characteristics.Health}\n" +
                    $"**{namesLocalization.Get("Mana")}:** {enemy.Characteristics.Mana}"),
                    new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("Embed/FightParams/Name"))
                    .WithValue($"**{namesLocalization.Get("AttackDamage")}:** {enemy.Characteristics.GetAttackDamage()}\n" +
                    $"**{namesLocalization.Get("AbilityPower")}:**** {enemy.Characteristics.GetAbilityPower()}\n" +
                    $"**{namesLocalization.Get("Armor")}:** {enemy.Characteristics.GetArmor()}\n" +
                    $"**{namesLocalization.Get("MagicResistance")}:** {enemy.Characteristics.GetMagicResistance()}\n" +
                    $"**{namesLocalization.Get("CriticalStrikeChance")}:** {enemy.Characteristics.GetCriticalStrikeChance()}%\n" +
                    $"**{namesLocalization.Get("CriticalStrikeDamageMultiplier")}:** {enemy.Characteristics.GetCriticalStrikeDamageMultiplier()}%")
                },
                "abilities" => new()
                {
                    new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("Embed/Abilities/Name"))
                    .WithValue($"{viewLocalization.Get("Embed/Abilities/Spell/Name").Format(enemy.Spell.Info.GetName(lang))}\n" +
                    $"{(enemy.Spell.HaveSpell() ?
                    $"{viewLocalization.Get("Embed/Abilities/Spell/MagicType").Format(enemy.Spell.MagicInfo.Info.GetName(lang))}\n" +
                    $"{viewLocalization.Get("Embed/Abilities/Spell/Desc").Format(enemy.Spell.Info.GetDescription(lang))}"
                    : string.Empty)}") 
                },
                "drop" => new()
                {
                    new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("Embed/Drop/Name"))
                    .WithValue(enemy.Drop.GetChancesInfo(dbUser.Characteristics.Luck, lang))
                },
                "actions" =>  new()
                {
                    new EmbedFieldBuilder()
                    .WithName(viewLocalization.Get("Embed/Action/Name"))
                    .WithValue($"{viewLocalization.Get("Embed/Action/LightAttack").Format(viewLocalization.Get($"Rate/{enemy.ActionChances.LightAttack}"))}\n" +
                    $"{viewLocalization.Get("Embed/Action/HeavyAttack").Format(viewLocalization.Get($"Rate/{enemy.ActionChances.HeavyAttack}"))}\n" +
                    $"{viewLocalization.Get("Embed/Action/Dodge").Format(viewLocalization.Get($"Rate/{enemy.ActionChances.Dodge}"))}\n" +
                    $"{viewLocalization.Get("Embed/Action/Defense").Format(viewLocalization.Get($"Rate/{enemy.ActionChances.Defense}"))}\n" +
                    $"{viewLocalization.Get("Embed/Action/Spell").Format(viewLocalization.Get($"Rate/{enemy.ActionChances.Spell}"))}\n")
                },
                _ => new() {
                    new EmbedFieldBuilder()
                    .WithName("** **")
                    .WithValue("** **") 
                }
            };
        }

        public static MessageComponent GetButtons(IDatabaseUser dbUser, IDatabaseEnemy enemy, Language lang)
        {
            var viewLocalization = _localizationPart.Get(lang);
            var result = new ComponentBuilder();
            var options = new List<SelectMenuOptionBuilder>();
            if (EnemyInfoService.CanViewEnemyCharacteristics(dbUser, enemy))
                options.Add(new(viewLocalization.Get("Select/Info/Option/Characteristics"), "characteristics"));
            if (EnemyInfoService.CanViewEnemyAbilities(dbUser, enemy))
                options.Add(new(viewLocalization.Get("Select/Info/Option/Abilites"), "abilities"));
            if (EnemyInfoService.CanViewEnemyDrop(dbUser, enemy))
                options.Add(new(viewLocalization.Get("Select/Info/Option/Drop"), "drop"));
            if (EnemyInfoService.CanViewEnemyActionChances(dbUser, enemy))
                options.Add(new(viewLocalization.Get("Select/Info/Option/Actions"), "actions"));
            if (options.Count > 0)
                return result
                    .WithSelectMenu("select", options, viewLocalization.Get("Select/Info/Name"), row: 0)
                    .WithButton(viewLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger, row: 1)
                    .Build();
            options.Add(new("EMPTY", "empty"));
            return result
                    .WithSelectMenu("select", options, viewLocalization.Get("Select/Info/NotFound"), disabled: true, row: 0)
                    .WithButton(viewLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger, row: 1)
                    .Build();

        }
    }
}
