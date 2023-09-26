using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Characteristics.Bonuses;
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
            {"Strength", 1 },
            {"Agility", 1 },
            {"Luck", 1 },
            {"Intellect", 1 },
            {"Endurance", 1 },
            {"Health", UserSettings.HealthPerUpPoint },
            {"Mana", UserSettings.ManaPerUpPoint }
        }
        );

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, bool endWithInfo)
        {
            var upgradeLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var bonuses = new CharacteristicBonuses();
            var points = dbUser.UpgradePoints.GetPoints();
            var maxPoints = points;
            var characteristicOptions = new List<SelectMenuOptionBuilder>()
            {
                new(namesLocalization.Get("Strength"), "Strength"),
                new(namesLocalization.Get("Agility"), "Agility"),
                new(namesLocalization.Get("Luck"), "Luck"),
                new(namesLocalization.Get("Intellect"), "Intellect"),
                new(namesLocalization.Get("Endurance"), "Endurance"),
                new(namesLocalization.Get("Health"), "Health"),
                new(namesLocalization.Get("Mana"), "Mana")
            };
            var arrow = characteristicOptions.First().Value;
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(upgradeLocalization.Get("Embed/Upgrade/Author"))
                    .WithDescription($"{upgradeLocalization.Get("Embed/Upgrade/Desc").Format(points)}\n\n" +
                    $"{IsChoosed(arrow, "Strength")}**{namesLocalization.Get("Strength")}:** " +
                    $"{dbUser.Characteristics.Strength.WithBonus(bonuses.Strength)}\n" +
                    $"{IsChoosed(arrow, "Agility")}**{namesLocalization.Get("Agility")}:** " +
                    $"{dbUser.Characteristics.Agility.WithBonus(bonuses.Agility)}\n" +
                    $"{IsChoosed(arrow, "Luck")}**{namesLocalization.Get("Luck")}:** " +
                    $"{dbUser.Characteristics.Luck.WithBonus(bonuses.Luck)}\n" +
                    $"{IsChoosed(arrow, "Intellect")}**{namesLocalization.Get("Intellect")}:** " +
                    $"{dbUser.Characteristics.Intellect.WithBonus(bonuses.Intellect)}\n" +
                    $"{IsChoosed(arrow, "Endurance")}**{namesLocalization.Get("Endurance")}:** " +
                    $"{dbUser.Characteristics.Endurance.WithBonus(bonuses.Endurance)}\n" +
                    $"{IsChoosed(arrow, "Health")}**{namesLocalization.Get("Health")}:** " +
                    $"{dbUser.Characteristics.Health.Max.WithBonus(bonuses.Health)}\n" +
                    $"{IsChoosed(arrow, "Mana")}**{namesLocalization.Get("Mana")}:** " +
                    $"{dbUser.Characteristics.Mana.Max.WithBonus(bonuses.Mana)}\n")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("select", characteristicOptions, upgradeLocalization.Get("Select/Name"), row: 0)
                    .WithButton("+1", "1", ButtonStyle.Success, disabled: points < 1, row: 1)
                    .WithButton("+2", "2", ButtonStyle.Success, disabled: points < 2, row: 1)
                    .WithButton("+5", "5", ButtonStyle.Success, disabled: points < 5, row: 1)
                    .WithButton(upgradeLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger, row: 2)
                    .WithButton(upgradeLocalization.Get("Btn/Reset"), "reset", ButtonStyle.Secondary, disabled: points == maxPoints, row: 2)
                    .WithButton(upgradeLocalization.Get("Btn/Confirm"), "confirm", ButtonStyle.Success, disabled: points == maxPoints, row: 2)
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
                        points -= amount;
                        switch (arrow)
                        {
                            case "Strength":
                                bonuses.Strength += _multipliers.GetValueOrDefault(arrow, 1) * amount;
                                break;
                            case "Agility":
                                bonuses.Agility += _multipliers.GetValueOrDefault(arrow, 1) * amount;
                                break;
                            case "Luck":
                                bonuses.Luck += _multipliers.GetValueOrDefault(arrow, 1) * amount;
                                break;
                            case "Intellect":
                                bonuses.Intellect += _multipliers.GetValueOrDefault(arrow, 1) * amount;
                                break;
                            case "Endurance":
                                bonuses.Endurance += _multipliers.GetValueOrDefault(arrow, 1) * amount;
                                break;
                            case "Health":
                                bonuses.Health += _multipliers.GetValueOrDefault(arrow, 1) * amount;
                                break;
                            case "Mana":
                                bonuses.Mana += _multipliers.GetValueOrDefault(arrow, 1) * amount;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "exit":
                        if (endWithInfo)
                        {
                            embed = EmbedFactory.CreateUserEmbed(dbUser, false, false)
                                .WithAuthor(upgradeLocalization.Get("Embed/UpgradeEnd/Author"))
                                .Build();
                            await context.Interaction.ModifyOriginalResponseAsync(msg =>
                            {
                                msg.Embed = embed;
                                msg.Components = new ComponentBuilder().Build();
                            });
                        }
                        return;
                    case "reset":
                        points = maxPoints;
                        bonuses = new CharacteristicBonuses();
                        break;
                    case "confirm":

                        if (endWithInfo)
                        {
                            embed = EmbedFactory.CreateUserEmbed(dbUser)
                                .WithAuthor(upgradeLocalization.Get("Embed/UpgradeEnd/Author"))
                                .WithDescription($"**{namesLocalization.Get("Strength")}:** {dbUser.Characteristics.Strength} " +
                                $"{(bonuses.Strength == 0 ? string.Empty : $"-> {dbUser.Characteristics.Strength + bonuses.Strength}")}\n" +
                                $"**{namesLocalization.Get("Agility")}:** {dbUser.Characteristics.Agility} " +
                                $"{(bonuses.Agility == 0 ? string.Empty : $"-> {dbUser.Characteristics.Agility + bonuses.Agility}")}\n" +
                                $"**{namesLocalization.Get("Luck")}:** {dbUser.Characteristics.Luck} " +
                                $"{(bonuses.Luck == 0 ? string.Empty : $"-> {dbUser.Characteristics.Luck + bonuses.Luck}")}\n" +
                                $"**{namesLocalization.Get("Intellect")}:** {dbUser.Characteristics.Intellect} " +
                                $"{(bonuses.Intellect == 0 ? string.Empty : $"-> {dbUser.Characteristics.Intellect + bonuses.Intellect}")}\n" +
                                $"**{namesLocalization.Get("Endurance")}:** {dbUser.Characteristics.Endurance} " +
                                $"{(bonuses.Endurance == 0 ? string.Empty : $"-> {dbUser.Characteristics.Endurance + bonuses.Endurance}")}\n" +
                                $"**{namesLocalization.Get("Health")}:** {dbUser.Characteristics.Health.Max} " +
                                $"{(bonuses.Health == 0 ? string.Empty : $"-> {dbUser.Characteristics.Health.Max + bonuses.Health}")}\n" +
                                $"**{namesLocalization.Get("Mana")}:** {dbUser.Characteristics.Mana.Max} " +
                                $"{(bonuses.Mana == 0 ? string.Empty : $"-> {dbUser.Characteristics.Mana.Max + bonuses.Mana}")}")
                            .Build();
                            await context.Interaction.ModifyOriginalResponseAsync(msg =>
                            {
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

        private static string IsChoosed(string choosed, string curent)
        {
            return choosed == curent ? EmojiService.Get("ChoosePoint").ToString()! : string.Empty;
        }
    }
}
