using Amazon.Runtime.Internal.Transform;
using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Characteristic;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;
using System.Collections.ObjectModel;

namespace Shuvi.CommandParts
{
    public static class UpgradePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("upgradePart");
        private static readonly IReadOnlyDictionary<string, int> _multipliers = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>() 
        { 
            {"0-0", 1 },
            {"0-1", 1 },
            {"0-2", 1 },
            {"0-3", 1 },
            {"0-4", 1 },
            {"1-0", UserSettings.HealthPerUpPoint },
            {"1-1", UserSettings.ManaPerUpPoint }
        }
        );

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, bool endWithInfo)
        {
            var upgradeLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var bonuses = new BonusesCharacteristics();
            var points = dbUser.UpgradePoints.GetPoints(dbUser);
            var maxPoints = points;
            var characteristicOptions = new List<SelectMenuOptionBuilder>()
            {
                new(namesLocalization.Get("strength"), "0-0"),
                new(namesLocalization.Get("agility"), "0-1"),
                new(namesLocalization.Get("luck"), "0-2"),
                new(namesLocalization.Get("intellect"), "0-3"),
                new(namesLocalization.Get("endurance"), "0-4"),
                new(namesLocalization.Get("health"), "1-0"),
                new(namesLocalization.Get("mana"), "1-1")
            };
            var arrow = characteristicOptions.First().Value;
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(upgradeLocalization.Get("embed/upgrade/author"))
                    .WithDescription($"{upgradeLocalization.Get("embed/upgrade/desc").Format(points)}\n\n" +
                    $"{(arrow == "0-0" ? EmojiService.Get("choosePoint") : string.Empty)}**{namesLocalization.Get("strength")}:** " +
                    $"{dbUser.Characteristics.Strength.WithBonus(bonuses.Strength)}\n" +
                    $"{(arrow == "0-1" ? EmojiService.Get("choosePoint") : string.Empty)}**{namesLocalization.Get("agility")}:** " +
                    $"{dbUser.Characteristics.Agility.WithBonus(bonuses.Agility)}\n" +
                    $"{(arrow == "0-2" ? EmojiService.Get("choosePoint") : string.Empty)}**{namesLocalization.Get("luck")}:** " +
                    $"{dbUser.Characteristics.Luck.WithBonus(bonuses.Luck)}\n" +
                    $"{(arrow == "0-3" ? EmojiService.Get("choosePoint") : string.Empty)}**{namesLocalization.Get("intellect")}:** " +
                    $"{dbUser.Characteristics.Intellect.WithBonus(bonuses.Intellect)}\n" +
                    $"{(arrow == "0-4" ? EmojiService.Get("choosePoint") : string.Empty)}**{namesLocalization.Get("endurance")}:** " +
                    $"{dbUser.Characteristics.Endurance.WithBonus(bonuses.Endurance)}\n" +
                    $"{(arrow == "1-0" ? EmojiService.Get("choosePoint") : string.Empty)}**{namesLocalization.Get("health")}:** " +
                    $"{dbUser.Characteristics.Health.Max.WithBonus(bonuses.Health)}\n" +
                    $"{(arrow == "1-1" ? EmojiService.Get("choosePoint") : string.Empty)}**{namesLocalization.Get("mana")}:** " +
                    $"{dbUser.Characteristics.Mana.Max.WithBonus(bonuses.Mana)}\n")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("select", characteristicOptions, upgradeLocalization.Get("select/name"), row: 0)
                    .WithButton("+1", "1", ButtonStyle.Success, disabled: points < 1, row: 1)
                    .WithButton("+2", "2", ButtonStyle.Success, disabled: points < 2, row: 1)
                    .WithButton("+5", "5", ButtonStyle.Success, disabled: points < 5, row: 1)
                    .WithButton(upgradeLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger, row: 2)
                    .WithButton(upgradeLocalization.Get("btn/reset"), "reset", ButtonStyle.Secondary, disabled: points == maxPoints, row: 2)
                    .WithButton(upgradeLocalization.Get("btn/confirm"), "confirm", ButtonStyle.Success, disabled: points == maxPoints, row: 2)
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
                        arrow = interaction.Data.Values.First();
                        break;
                    case "1" or "2" or "5":
                        var amount = int.Parse(interaction.Data.CustomId);
                        var codes = arrow.Split("-");
                        points -= amount;
                        switch (codes[0])
                        {
                            case "0":
                                bonuses.Add((StaticCharacteristic)int.Parse(codes[1]), 
                                    _multipliers.GetValueOrDefault(arrow, 1) * amount);
                                break;
                            case "1":
                                bonuses.Add((DynamicCharacteristic)int.Parse(codes[1]),
                                    _multipliers.GetValueOrDefault(arrow, 1) * amount);
                                break;
                        }
                        break;
                    case "exit":
                        if (endWithInfo)
                        {
                            embed = EmbedFactory.CreateUserEmbed(context.User, dbUser, false, false)
                                .WithAuthor(upgradeLocalization.Get("embed/upgradeEnd/author"))
                                .Build();
                            await context.Interaction.ModifyOriginalResponseAsync(msg => {
                                msg.Embed = embed;
                                msg.Components = new ComponentBuilder().Build();
                            });
                        }
                        return;
                    case "reset":
                        points = maxPoints;
                        bonuses = new BonusesCharacteristics();
                        break;
                    case "confirm":

                        if (endWithInfo)
                        {
                            embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                                .WithAuthor(upgradeLocalization.Get("embed/upgradeEnd/author"))
                                .WithDescription($"**{namesLocalization.Get("strength")}:** {dbUser.Characteristics.Strength} " +
                                $"{(bonuses.Strength == 0 ? string.Empty : $"-> {dbUser.Characteristics.Strength + bonuses.Strength}")}\n" +
                                $"**{namesLocalization.Get("agility")}:** {dbUser.Characteristics.Agility} " +
                                $"{(bonuses.Agility == 0 ? string.Empty : $"-> {dbUser.Characteristics.Agility + bonuses.Agility}")}\n" +
                                $"**{namesLocalization.Get("luck")}:** {dbUser.Characteristics.Luck} " +
                                $"{(bonuses.Luck == 0 ? string.Empty : $"-> {dbUser.Characteristics.Luck + bonuses.Luck}")}\n" +
                                $"**{namesLocalization.Get("intellect")}:** {dbUser.Characteristics.Intellect} " +
                                $"{(bonuses.Intellect == 0 ? string.Empty : $"-> {dbUser.Characteristics.Intellect + bonuses.Intellect}")}\n" +
                                $"**{namesLocalization.Get("endurance")}:** {dbUser.Characteristics.Endurance} " +
                                $"{(bonuses.Endurance == 0 ? string.Empty : $"-> {dbUser.Characteristics.Endurance + bonuses.Endurance}")}\n" +
                                $"**{namesLocalization.Get("health")}:** {dbUser.Characteristics.Health.Max} " +
                                $"{(bonuses.Health == 0 ? string.Empty : $"-> {dbUser.Characteristics.Health.Max + bonuses.Health}")}\n" +
                                $"**{namesLocalization.Get("mana")}:** {dbUser.Characteristics.Mana.Max} " +
                                $"{(bonuses.Mana == 0 ? string.Empty : $"-> {dbUser.Characteristics.Mana.Max + bonuses.Mana}")}")
                            .Build();
                            await context.Interaction.ModifyOriginalResponseAsync(msg => { 
                                msg.Embed = embed; 
                                msg.Components = new ComponentBuilder().Build(); 
                            });
                        }
                        dbUser.Characteristics.Add(bonuses);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Strength, dbUser.Characteristics.Strength)
                            .Set(x => x.Agility, dbUser.Characteristics.Agility)
                            .Set(x => x.Luck, dbUser.Characteristics.Luck)
                            .Set(x => x.Intellect, dbUser.Characteristics.Intellect)
                            .Set(x => x.Endurance, dbUser.Characteristics.Endurance)
                            .Set(x => x.MaxHealth, dbUser.Characteristics.Health.Max)
                            .Set(x => x.MaxMana, dbUser.Characteristics.Mana.Max));
                        return;
                }
            }
        }
    }
}
