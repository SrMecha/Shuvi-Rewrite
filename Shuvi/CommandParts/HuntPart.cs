using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Combat;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class HuntPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("huntPart");
        public const int HuntEnergyCost = 1;

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var huntLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            while (context.LastInteraction is not null)
            {
                dbUser.Characteristics.Energy.Reduce(HuntEnergyCost);
                await UserDatabase.UpdateUser(dbUser.Id,
                    new UpdateDefinitionBuilder<UserData>().Set(x => x.EnergyRegenTime, dbUser.Characteristics.Energy.RegenTime));
                var enemy = EnemyDatabase.GetEnemy(dbUser.Location.GetLocation().Enemies.GetRandom());
                var combatEnemy = new CombatEnemy(enemy, context.Language);
                var canViewStats = EnemyInfoService.CanViewEnemyCharacteristics(dbUser, enemy);
                var canViewSpell = EnemyInfoService.CanViewEnemyAbilities(dbUser, enemy);
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(huntLocalization.Get("Embed/Search/Author"))
                    .WithDescription($"{huntLocalization.Get("Embed/Search/Meet").Format(enemy.Info.GetName(context.Language))}\n" +
                    $"{huntLocalization.Get("Embed/Search/EnergyRemaining").Format(dbUser.Characteristics.Energy.GetCurrent(),
                    dbUser.Characteristics.Energy.Max)}")
                    .AddField(enemy.Info.GetName(context.Language),
                    canViewStats ? $"**{namesLocalization.Get("Strength")}** {combatEnemy.Characteristics.Strength}\n" +
                    $"**{namesLocalization.Get("Agility")}:** {combatEnemy.Characteristics.Agility}\n" +
                    $"**{namesLocalization.Get("Luck")}:** {combatEnemy.Characteristics.Luck}\n" +
                    $"**{namesLocalization.Get("Intellect")}:** {combatEnemy.Characteristics.Intellect}\n" +
                    $"**{namesLocalization.Get("Endurance")}:** {combatEnemy.Characteristics.Endurance}\n" +
                    $"**{namesLocalization.Get("Health")}:** {combatEnemy.Characteristics.Health.Max}/{combatEnemy.Characteristics.Health.Max}\n" +
                    $"**{namesLocalization.Get("Mana")}:** {combatEnemy.Characteristics.Mana.Max}/{combatEnemy.Characteristics.Mana.Max}" :
                    huntLocalization.Get("Embed/Search/CharacteristicsUnknown"),
                    true)
                    .AddField(huntLocalization.Get("Embed/Search/Info"),
                    $"{huntLocalization.Get("Embed/Search/Rank").Format(enemy.Rank.ToString())}\n" +
                    $"{(canViewSpell ? huntLocalization.Get("Embed/Search/Spell").Format(enemy.Spell.Info.GetName(context.Language)) :
                    huntLocalization.Get("Embed/Search/SpellUnknown"))}",
                    true)
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(huntLocalization.Get("Btn/Attack"), "attack", ButtonStyle.Danger, row: 0)
                    //.WithButton(huntLocalization.Get("Btn/Tame"), "tame", ButtonStyle.Success, row: 0)
                    .WithButton(huntLocalization.Get("Btn/Retry"), "retry", ButtonStyle.Secondary,
                    disabled: !dbUser.Characteristics.HaveEnergy(HuntEnergyCost), row: 1)
                    .WithButton(huntLocalization.Get("Btn/Leave"), "leave", ButtonStyle.Secondary, row: 2)
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
                    case "attack":
                        await PlayerVersusEnemyPart.Start(context, dbUser, enemy);
                        return;
                    case "tame":
                        return;
                    case "retry":
                        break;
                    case "leave":
                        embed = EmbedFactory.CreateUserEmbed(dbUser)
                            .WithDescription(huntLocalization.Get("Embed/Leave/Desc"))
                            .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg =>
                        {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        await context.LastInteraction.TryDeferAsync();
                        return;
                }
            }
        }
    }
}
